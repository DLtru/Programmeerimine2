using KooliProjekt.Data;
using KooliProjekt.Models;
using KooliProjekt.Search;
using KooliProjekt.Services;
using System;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace KooliProjekt.UnitTests.ServiceTests
{
    public class LogEntryServiceTests : ServiceTestBase
    {
        private readonly LogEntryService _service;

        public LogEntryServiceTests()
        {
            _service = new LogEntryService(DbContext);
        }

        [Fact]
        public async Task List_should_return_all_log_entries()
        {
            // Arrange
            DbContext.LogEntries.AddRange(
                new LogEntry { Description = "Log A", Date = DateTime.Today },
                new LogEntry { Description = "Log B", Date = DateTime.Today.AddDays(1) }
            );
            DbContext.SaveChanges();

            // Act
            var result = await _service.List(1, 10, new LogEntriesSearch());

            // Assert
            Assert.NotNull(result);
            Assert.NotNull(result.Results);
            Assert.Equal(2, result.Results.Count);
        }

        [Fact]
        public async Task List_should_filter_by_keyword()
        {
            // Arrange
            DbContext.LogEntries.AddRange(
                new LogEntry { Description = "Error Log", Date = DateTime.Today },
                new LogEntry { Description = "Warning Log", Date = DateTime.Today }
            );
            DbContext.SaveChanges();

            // Act
            var result = await _service.List(1, 10, new LogEntriesSearch { Keyword = "Error" });

            // Assert
            Assert.NotNull(result);
            Assert.Single(result.Results);
            Assert.Equal("Error Log", result.Results[0].Description);
        }

        [Fact]
        public async Task List_should_filter_by_start_date()
        {
            // Arrange
            var today = DateTime.Today;
            DbContext.LogEntries.AddRange(
                new LogEntry { Description = "Log A", Date = today.AddDays(-2) },
                new LogEntry { Description = "Log B", Date = today }
            );
            DbContext.SaveChanges();

            // Act
            var result = await _service.List(1, 10, new LogEntriesSearch { StartDate = today });

            // Assert
            Assert.NotNull(result);
            Assert.Single(result.Results);
            Assert.Equal("Log B", result.Results[0].Description);
        }

        [Fact]
        public async Task List_should_filter_by_end_date()
        {
            // Arrange
            var today = DateTime.Today;
            DbContext.LogEntries.AddRange(
                new LogEntry { Description = "Log A", Date = today },
                new LogEntry { Description = "Log B", Date = today.AddDays(2) }
            );
            DbContext.SaveChanges();

            // Act
            var result = await _service.List(1, 10, new LogEntriesSearch { EndDate = today });

            // Assert
            Assert.NotNull(result);
            Assert.Single(result.Results);
            Assert.Equal("Log A", result.Results[0].Description);
        }
    }
}