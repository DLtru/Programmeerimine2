using KooliProjekt.Controllers;
using KooliProjekt.Data;
using KooliProjekt.Search;
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
    public class BeersControllerTests
    {
        private readonly Mock<IBeerService> beerServiceMock;
        private readonly BeersController controller;

        public BeersControllerTests()
        {
            beerServiceMock = new Mock<IBeerService>();
            controller = new BeersController(beerServiceMock.Object);
        }

        [Fact]
        public async Task Index_Should_Return_Correct_View_With_Data()
        {
            // Arrange
            int page = 1;
            var data = new List<Beer>
            {
                new Beer { Id = 1, Name = "Beer 1", AlcoholPercentage = 5.0 },
                new Beer { Id = 2, Name = "Beer 2", AlcoholPercentage = 4.5 }
            };
            var pagedResult = new PagedResult<Beer>
            {
                Results = data,
                CurrentPage = 1,
                PageCount = 1,
                PageIndex = 1,
                PageNumber = 1
            };
            beerServiceMock.Setup(x => x.List(page, It.IsAny<int>(), It.IsAny<BeerSearch>())).ReturnsAsync(pagedResult);

            // Act
            var result = await controller.Index(page) as ViewResult;

            // Assert
            Assert.NotNull(result);
            Assert.NotNull(result.Model);
            Assert.IsType<PagedResult<Beer>>(result.Model);
            var model = (PagedResult<Beer>)result.Model;
            Assert.NotNull(model);
            Assert.Equal(pagedResult.Results.Count, model.Results.Count);
            Assert.Equal(pagedResult.Results[0].Name, model.Results[0].Name);
        }

        [Fact]
        public async Task Details_Should_Return_NotFound_When_Id_Is_Null()
        {
            // Arrange
            int? id = null;

            // Act
            var result = await controller.Details(id) as NotFoundResult;

            // Assert
            Assert.NotNull(result);
        }

        [Fact]
        public async Task Details_Should_Return_Correct_View_With_Model()
        {
            // Arrange
            var beer = new Beer { Id = 1, Name = "Beer 1", AlcoholPercentage = 5.0 };
            beerServiceMock.Setup(x => x.GetBeerByIdAsync(1)).ReturnsAsync(beer); // Используем правильный метод

            // Act
            var result = await controller.Details(1) as ViewResult;

            // Assert
            Assert.NotNull(result); // Убедитесь, что результат не null
            Assert.NotNull(result.Model); // Убедитесь, что модель не null
            Assert.IsType<Beer>(result.Model); // Проверяем, что модель типа Beer
            var model = (Beer)result.Model;
            Assert.Equal(beer.Id, model.Id); // Проверяем, что Id совпадает
            Assert.Equal(beer.Name, model.Name); // Проверяем, что Name совпадает
        }
    }
}
