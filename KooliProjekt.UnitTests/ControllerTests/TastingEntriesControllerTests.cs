using KooliProjekt.Controllers;
using KooliProjekt.Data;
using KooliProjekt.Models;
using KooliProjekt.Services;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace KooliProjekt.UnitTests.ControllerTests
{
    public class TastingEntriesControllerTests
    {
        private readonly Mock<ITastingEntryService> _tastingEntryServiceMock;
        private readonly TastingEntriesController _controller;

        public TastingEntriesControllerTests()
        {
            _tastingEntryServiceMock = new Mock<ITastingEntryService>();
            _controller = new TastingEntriesController(_tastingEntryServiceMock.Object);
        }

        [Fact]
        public async Task Index_should_return_correct_view_with_data()
        {
            // Arrange
            int page = 1;
            var data = new List<TastingEntry>
            {
                new TastingEntry { Id = 1, Title = "Test 1" },
                new TastingEntry { Id = 2, Title = "Test 2" }
            };
            var pagedResult = new PagedResult<TastingEntry> { Results = data };
            var model = new TastingEntriesIndexModel { Data = pagedResult };
            _tastingEntryServiceMock.Setup(x => x.List(page, It.IsAny<int>(), null)).ReturnsAsync(model);

            // Act
            var result = await _controller.Index(null, page) as ViewResult;

            // Assert
            Assert.NotNull(result);
            var viewModel = Assert.IsType<TastingEntriesIndexModel>(result.Model);
            Assert.Equal(data.Count, viewModel.Data.Results.Count);
        }
    }
}
