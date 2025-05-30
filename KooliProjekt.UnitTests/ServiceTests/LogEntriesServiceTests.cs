using KooliProjekt.Data;
using KooliProjekt.Models;
using KooliProjekt.Search;
using KooliProjekt.Services;
using System;
using System.Threading.Tasks;
using Xunit;

namespace KooliProjekt.UnitTests.ServiceTests
{
    public class LogEntriesServiceTests : HomeServiceTestBase
    {
        private readonly ILogEntryService _service;
        private readonly DateTime _currentDate = DateTime.Parse("2025-05-30 11:13:49");
        private const string _currentUser = "DLtru";

        public LogEntriesServiceTests()
        {
            _service = new LogEntryService(DbContext);
        }

        [Fact]
        public async Task List_should_return_all_log_entries()
        {
            // Arrange
            var entries = new[]
            {
                new LogEntry { Description = "Test 1", Date = _currentDate, UserId = _currentUser },
                new LogEntry { Description = "Test 2", Date = _currentDate, UserId = _currentUser }
            };

            DbContext.LogEntries.AddRange(entries);
            await DbContext.SaveChangesAsync();

            // Act
            var result = await _service.List(1, 10, new LogEntriesSearch());

            // Assert
            Assert.NotNull(result);
        }

        [Fact]
        public async Task Get_should_return_entry_by_id()
        {
            // Arrange
            var entry = new LogEntry
            {
                Description = "Test Entry",
                Date = _currentDate,
                UserId = _currentUser
            };
            DbContext.LogEntries.Add(entry);
            await DbContext.SaveChangesAsync();

            // Act
            var result = await _service.Get(entry.Id);

            // Assert
            Assert.NotNull(result);
            Assert.Equal("Test Entry", result.Description);
        }

        [Fact]
        public async Task Save_should_add_new_entry()
        {
            // Arrange
            var entry = new LogEntry
            {
                Description = "New Entry",
                Date = _currentDate,
                UserId = _currentUser
            };

            // Act
            await _service.Save(entry);

            // Assert
            var saved = await DbContext.LogEntries.FindAsync(entry.Id);
            Assert.NotNull(saved);
            Assert.Equal("New Entry", saved.Description);
        }

        [Fact]
        public async Task Save_should_update_existing_entry()
        {
            // Arrange
            var entry = new LogEntry
            {
                Description = "Original Entry",
                Date = _currentDate,
                UserId = _currentUser
            };
            DbContext.LogEntries.Add(entry);
            await DbContext.SaveChangesAsync();

            // Act
            entry.Description = "Updated Entry";
            await _service.Save(entry);

            // Assert
            var updated = await DbContext.LogEntries.FindAsync(entry.Id);
            Assert.NotNull(updated);
            Assert.Equal("Updated Entry", updated.Description);
        }

        [Fact]
        public async Task Delete_should_remove_existing_entry()
        {
            // Arrange
            var entry = new LogEntry
            {
                Description = "To Delete",
                Date = _currentDate,
                UserId = _currentUser
            };
            DbContext.LogEntries.Add(entry);
            await DbContext.SaveChangesAsync();

            // Act
            await _service.Delete(entry.Id);

            // Assert
            var deleted = await DbContext.LogEntries.FindAsync(entry.Id);
            Assert.Null(deleted);
        }

        [Fact]
        public async Task Delete_should_not_throw_for_non_existing_entry()
        {
            // Act & Assert
            await _service.Delete(999); // Не должно вызывать исключение
        }
    }
}