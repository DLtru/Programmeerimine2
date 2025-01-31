using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KooliProjekt.Controllers;
using KooliProjekt.Data;
using KooliProjekt.Services;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

namespace KooliProjekt.UnitTests.ControllerTests
{
    public class LogEntriesControllerTests
    {
        private readonly Mock<ILogEntryService> _LogEntryServiceMock;
        private readonly LogEntriesController _controller;

        public LogEntriesControllerTests()
        {
            _LogEntryServiceMock = new Mock<ILogEntryService>();
            _controller = new LogEntriesController(_LogEntryServiceMock.Object);
        }

        [Fact]
        public async Task Index_should_return_correct_view_with_data()
        {
            // Arrange
            int page = 1;
            var data = new List<LogEntry>
            {
                new LogEntry { Id = 1, Title = "Test 1" },
                new LogEntry { Id = 2, Title = "Test 2" }
            };
            var pagedResult = new PagedResult<LogEntry> { Results = data };
            _LogEntryServiceMock.Setup(x => x.List(page, It.IsAny<int>(), null)).ReturnsAsync(pagedResult);

            // Act
            var result = await _controller.Index(page) as ViewResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal(pagedResult, result.Model);
        }
    }
}