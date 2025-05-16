using KooliProjekt.Data;
using KooliProjekt.Models;
using KooliProjekt.Services;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using Xunit;

namespace KooliProjekt.Tests.ServiceTests
{
    public class PhotoServiceTests
    {
        private ApplicationDbContext GetMemoryContext()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            var context = new ApplicationDbContext(options);
            return context;
        }

        [Fact]
        public void Get_ReturnsCorrectPhoto()
        {
            // Arrange
            var context = GetMemoryContext();
            var photo = new Photo { Id = 1, FileName = "test.jpg", ContentType = "image/jpeg" };
            context.Photos.Add(photo);
            context.SaveChanges();

            var service = new PhotoService(context);

            // Act
            var result = service.Get(1);

            // Assert
            Assert.NotNull(result);
            Assert.Equal("test.jpg", result.FileName);
            Assert.Equal("image/jpeg", result.ContentType);
        }

        [Fact]
        public void List_ReturnsAllPhotos()
        {
            // Arrange
            var context = GetMemoryContext();
            context.Photos.Add(new Photo { Id = 1, FileName = "photo1.jpg", ContentType = "image/jpeg" });
            context.Photos.Add(new Photo { Id = 2, FileName = "photo2.png", ContentType = "image/png" });
            context.SaveChanges();

            var service = new PhotoService(context);

            // Act
            var result = service.List();

            // Assert
            Assert.Equal(2, result.Count());
        }

        [Fact]
        public void Save_AddsNewPhoto()
        {
            // Arrange
            var context = GetMemoryContext();
            var service = new PhotoService(context);
            var photo = new Photo { FileName = "new.jpg", ContentType = "image/jpeg" };

            // Act
            service.Save(photo);

            // Assert
            Assert.Equal(1, context.Photos.Count());
            Assert.NotEqual(0, photo.Id);
        }

        [Fact]
        public void Save_UpdatesExistingPhoto()
        {
            // Arrange
            var context = GetMemoryContext();
            var photo = new Photo { Id = 1, FileName = "old.jpg", ContentType = "image/jpeg" };
            context.Photos.Add(photo);
            context.SaveChanges();

            var service = new PhotoService(context);
            photo.FileName = "updated.jpg";
            photo.ContentType = "image/png";

            // Act
            service.Save(photo);

            // Assert
            var updatedPhoto = context.Photos.Find(1);
            Assert.Equal("updated.jpg", updatedPhoto.FileName);
            Assert.Equal("image/png", updatedPhoto.ContentType);
        }

        [Fact]
        public void Delete_RemovesNonExistingPhoto()
        {
            // Arrange
            var context = GetMemoryContext();
            var service = new PhotoService(context);
            var photo = new Photo { Id = 999, FileName = "test.jpg", ContentType = "image/jpeg" };

            // Act & Assert
            // Should not throw for non-existing photo
            service.Delete(photo);
        }

        [Fact]
        public void Delete_RemovesExistingPhoto()
        {
            // Arrange
            var context = GetMemoryContext();
            var photo = new Photo { Id = 1, FileName = "test.jpg", ContentType = "image/jpeg" };
            context.Photos.Add(photo);
            context.SaveChanges();

            var service = new PhotoService(context);

            // Act
            service.Delete(photo);

            // Assert
            Assert.Equal(0, context.Photos.Count());
        }
    }
}