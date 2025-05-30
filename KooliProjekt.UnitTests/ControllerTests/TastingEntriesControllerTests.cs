using KooliProjekt.Controllers;
using KooliProjekt.Data;
using KooliProjekt.Models;
using KooliProjekt.Search;
using KooliProjekt.Services;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace KooliProjekt.UnitTests.ControllerTests
{
    public class TastingEntriesControllerTests
    {
        private readonly Mock<ITastingEntryService> _mockService;
        private readonly TastingEntriesController _controller;

        public TastingEntriesControllerTests()
        {
            _mockService = new Mock<ITastingEntryService>();
            _controller = new TastingEntriesController(_mockService.Object);
        }

        [Fact]
        public async Task Index_ReturnsViewWithModel()
        {
            // Arrange
            var pagedResult = new PagedResult<TastingEntry>
            {
                Data = new PagedList<TastingEntry>
                {
                    Results = new List<TastingEntry>(),
                    Total = 0
                }
            };

            _mockService
                .Setup(s => s.List(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<TastingEntriesSearch>()))
                .Returns(Task.FromResult(pagedResult));

            // Act
            var result = await _controller.Index(new TastingEntriesSearch(), 1) as ViewResult;

            // Assert
            Assert.NotNull(result);
            Assert.NotNull(result.Model);
        }
    }
}