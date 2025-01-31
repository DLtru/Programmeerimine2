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
    public class BeersControllerTests
    {
        private readonly Mock<IBeerService> _BeerServiceMock;
        private readonly BeersController _controller;

        public BeersControllerTests()
        {
            _BeerServiceMock = new Mock<IBeerService>();
            _controller = new BeersController(_BeerServiceMock.Object);
        }

        [Fact]
        public async Task Index_should_return_correct_view_with_data()
        {
            // Arrange
            int page = 1;
            var data = new List<Beer>
            {
                new Beer { Id = 1, Title = "Test 1" },
                new Beer { Id = 2, Title = "Test 2" }
            };
            var pagedResult = new PagedResult<Beer> { Results = data };
            _BeerServiceMock.Setup(x => x.List(page, It.IsAny<int>(), null)).ReturnsAsync(pagedResult);

            // Act
            var result = await _controller.Index(page) as ViewResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal(pagedResult, result.Model);
        }
    }
}