using KooliProjekt.Data;
using KooliProjekt.Models;
using KooliProjekt.Search;
using KooliProjekt.Services;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace KooliProjekt.UnitTests.ServiceTests
{
    public class PhotoServiceTests
    {
        private readonly ApplicationDbContext _context;
        private readonly PhotoService _service;

        public PhotoServiceTests()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            _context = new ApplicationDbContext(options);
            _service = new PhotoService(_context);
        }

        [Fact]
        public async Task GetPhotoById_ReturnsCorrectPhoto()
        {
            // Arrange
            var photo = new Photo
            {
                Description = "Test Photo",
                Date = DateTime.Now,
                Title = "Test Title",
                Url = "http://test.com/photo.jpg",
                CreatedDate = DateTime.Now
            };
            _context.Photos.Add(photo);
            await _context.SaveChangesAsync();

            // Act
            var result = await _service.GetPhotoByIdAsync(photo.Id);

            // Assert
            Assert.NotNull(result);
            Assert.Equal("Test Photo", result.Description);
            Assert.Equal("Test Title", result.Title);
            Assert.Equal("http://test.com/photo.jpg", result.Url);
        }

        [Fact]
        public async Task GetPhotoById_ReturnsNullForInvalidId()
        {
            // Act
            var result = await _service.GetPhotoByIdAsync(-1);

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public async Task List_ReturnsAllPhotos()
        {
            // Arrange
            var search = new PhotosSearch();
            var currentDate = DateTime.Now;

            await _context.Photos.AddRangeAsync(
                new Photo
                {
                    Description = "Photo 1",
                    Date = currentDate,
                    Title = "Title 1",
                    Url = "http://test.com/photo1.jpg",
                    CreatedDate = currentDate
                },
                new Photo
                {
                    Description = "Photo 2",
                    Date = currentDate,
                    Title = "Title 2",
                    Url = "http://test.com/photo2.jpg",
                    CreatedDate = currentDate
                }
            );
            await _context.SaveChangesAsync();

            // Act
            var result = await _service.List(1, 10, search);

            // Assert
            Assert.NotNull(result);
            Assert.NotNull(result.Results);  // Changed from Items to Results
            Assert.Equal(2, result.Results.Count());  // Changed from Items to Results
        }

        [Fact]
        public async Task List_FiltersPhotosByTitle()
        {
            // Arrange
            var currentDate = DateTime.Now;
            await _context.Photos.AddRangeAsync(
                new Photo
                {
                    Description = "Target Description",
                    Title = "Target Photo",
                    Date = currentDate,
                    Url = "http://test.com/target.jpg",
                    CreatedDate = currentDate
                },
                new Photo
                {
                    Description = "Other Description",
                    Title = "Other Photo",
                    Date = currentDate,
                    Url = "http://test.com/other.jpg",
                    CreatedDate = currentDate
                }
            );
            await _context.SaveChangesAsync();

            var search = new PhotosSearch { Title = "Target" };

            // Act
            var result = await _service.List(1, 10, search);

            // Assert
            Assert.Single(result.Results);  // Changed from Items to Results
            Assert.Contains(result.Results, p => p.Description == "Target Description");  // Changed from Items to Results
        }

        [Fact]
        public async Task List_FiltersPhotosByDateRange()
        {
            // Arrange
            var date1 = new DateTime(2025, 1, 1);
            var date2 = new DateTime(2025, 2, 1);
            var date3 = new DateTime(2025, 3, 1);

            await _context.Photos.AddRangeAsync(
                new Photo
                {
                    Description = "Photo 1",
                    Title = "Title 1",
                    Date = date1,
                    Url = "http://test.com/photo1.jpg",
                    CreatedDate = date1
                },
                new Photo
                {
                    Description = "Photo 2",
                    Title = "Title 2",
                    Date = date2,
                    Url = "http://test.com/photo2.jpg",
                    CreatedDate = date2
                },
                new Photo
                {
                    Description = "Photo 3",
                    Title = "Title 3",
                    Date = date3,
                    Url = "http://test.com/photo3.jpg",
                    CreatedDate = date3
                }
            );
            await _context.SaveChangesAsync();

            var search = new PhotosSearch
            {
                StartDate = date1,
                EndDate = date2
            };

            // Act
            var result = await _service.List(1, 10, search);

            // Assert
            Assert.Equal(2, result.Results.Count());  // Changed from Items to Results
            Assert.DoesNotContain(result.Results, p => p.Date > date2);  // Changed from Items to Results
            Assert.DoesNotContain(result.Results, p => p.Date < date1);  // Changed from Items to Results
        }

        [Fact]
        public async Task CreatePhoto_AddsNewPhoto()
        {
            // Arrange
            var currentDate = DateTime.Now;
            var photo = new Photo
            {
                Description = "New Photo",
                Title = "New Title",
                Date = currentDate,
                Url = "http://test.com/new.jpg",
                CreatedDate = currentDate
            };

            // Act
            await _service.CreatePhotoAsync(photo);

            // Assert
            Assert.Equal(1, _context.Photos.Count());
            Assert.NotEqual(0, photo.Id);
            var savedPhoto = await _context.Photos.FindAsync(photo.Id);
            Assert.Equal("New Photo", savedPhoto.Description);
            Assert.Equal("New Title", savedPhoto.Title);
        }

        [Fact]
        public async Task UpdatePhoto_ModifiesExistingPhoto()
        {
            // Arrange
            var currentDate = DateTime.Now;
            var photo = new Photo
            {
                Description = "Original Photo",
                Title = "Original Title",
                Date = currentDate,
                Url = "http://test.com/original.jpg",
                CreatedDate = currentDate
            };
            _context.Photos.Add(photo);
            await _context.SaveChangesAsync();

            photo.Description = "Updated Photo";
            photo.Title = "Updated Title";
            photo.Url = "http://test.com/updated.jpg";

            // Act
            await _service.UpdatePhotoAsync(photo);

            // Assert
            var updatedPhoto = await _context.Photos.FindAsync(photo.Id);
            Assert.Equal("Updated Photo", updatedPhoto.Description);
            Assert.Equal("Updated Title", updatedPhoto.Title);
            Assert.Equal("http://test.com/updated.jpg", updatedPhoto.Url);
        }

        [Fact]
        public async Task DeletePhoto_RemovesExistingPhoto()
        {
            // Arrange
            var currentDate = DateTime.Now;
            var photo = new Photo
            {
                Description = "Photo to Delete",
                Title = "Delete Title",
                Date = currentDate,
                Url = "http://test.com/delete.jpg",
                CreatedDate = currentDate
            };
            _context.Photos.Add(photo);
            await _context.SaveChangesAsync();

            // Act
            await _service.DeletePhotoAsync(photo.Id);

            // Assert
            Assert.Equal(0, _context.Photos.Count());
        }

        [Fact]
        public async Task DeletePhoto_HandlesNonExistentPhoto()
        {
            // Act & Assert
            // Should not throw exception
            await _service.DeletePhotoAsync(-999);
            Assert.Equal(0, _context.Photos.Count());
        }
    }
}