using KooliProjekt.Data;
using KooliProjekt.Services;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace KooliProjekt.Tests.ServiceTests
{
    public class TastingEntryServiceTests
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
        public void Get_ReturnsCorrectTastingEntry()
        {
            // Arrange
            var context = GetMemoryContext();
            var tastingEntry = new TastingEntry { Id = 1, Comments = "Test Comments", Rating = 5 };
            context.TastingEntries.Add(tastingEntry);
            context.SaveChanges();

            var service = new TastingEntryService(context);

            // Act
            var result = service.Get(1);

            // Assert
            Assert.NotNull(result);
            Assert.Equal("Test Comments", result.Comments);
            Assert.Equal(5, result.Rating);
        }

        [Fact]
        public void List_ReturnsAllTastingEntries()
        {
            // Arrange
            var context = GetMemoryContext();
            context.TastingEntries.Add(new TastingEntry { Id = 1, Comments = "Entry 1", Rating = 4 });
            context.TastingEntries.Add(new TastingEntry { Id = 2, Comments = "Entry 2", Rating = 5 });
            context.SaveChanges();

            var service = new TastingEntryService(context);

            // Act
            var result = service.List();

            // Assert
            Assert.Equal(2, result.Count());
        }

        [Fact]
        public void Save_AddsNewTastingEntry()
        {
            // Arrange
            var context = GetMemoryContext();
            var service = new TastingEntryService(context);
            var entry = new TastingEntry { Comments = "New Entry", Rating = 4 };

            // Act
            service.Save(entry);

            // Assert
            Assert.Equal(1, context.TastingEntries.Count());
            Assert.NotEqual(0, entry.Id);
        }

        [Fact]
        public void Save_UpdatesExistingTastingEntry()
        {
            // Arrange
            var context = GetMemoryContext();
            var entry = new TastingEntry { Id = 1, Comments = "Old Comments", Rating = 3 };
            context.TastingEntries.Add(entry);
            context.SaveChanges();

            var service = new TastingEntryService(context);
            entry.Comments = "Updated Comments";
            entry.Rating = 5;

            // Act
            service.Save(entry);

            // Assert
            var updatedEntry = context.TastingEntries.Find(1);
            Assert.Equal("Updated Comments", updatedEntry.Comments);
            Assert.Equal(5, updatedEntry.Rating);
        }

        [Fact]
        public void Delete_RemovesNonExistingTastingEntry()
        {
            // Arrange
            var context = GetMemoryContext();
            var service = new TastingEntryService(context);
            var entry = new TastingEntry { Id = 999, Comments = "Test Comments", Rating = 4 };

            // Act & Assert
            // Should not throw for non-existing entry
            service.Delete(entry);
        }

        [Fact]
        public void Delete_RemovesExistingTastingEntry()
        {
            // Arrange
            var context = GetMemoryContext();
            var entry = new TastingEntry { Id = 1, Comments = "Test Comments", Rating = 4 };
            context.TastingEntries.Add(entry);
            context.SaveChanges();

            var service = new TastingEntryService(context);

            // Act
            service.Delete(entry);

            // Assert
            Assert.Equal(0, context.TastingEntries.Count());
        }
    }
}