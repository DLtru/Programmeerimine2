using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KooliProjekt.Controllers;
using KooliProjekt.Data;
using KooliProjekt.Models;
using KooliProjekt.Services;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

namespace KooliProjekt.UnitTests.ControllerTests
{
    public class TastingEntriesControllerTests
    {
        private readonly Mock<ITastingEntryService> _TastingEntryServiceMock;
        private readonly TastingEntriesController _controller;

        public TastingEntriesControllerTests()
        {
            _TastingEntryServiceMock = new Mock<ITastingEntryService>();
            _controller = new TastingEntriesController(_TastingEntryServiceMock.Object);
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
            _TastingEntryServiceMock.Setup(x => x.List(page, It.IsAny<int>(), null)).ReturnsAsync(model);

            // Act
            var result = await _controller.Index(page) as ViewResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal(pagedResult, result.Model);
        }
    }
}