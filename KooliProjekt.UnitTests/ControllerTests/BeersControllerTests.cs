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

            var result = await controller.Index(page) as ViewResult;

            Assert.NotNull(result);
            Assert.NotNull(result.Model);
            Assert.IsType<PagedResult<Beer>>(result.Model);
            var model = (PagedResult<Beer>)result.Model;
            Assert.Equal(pagedResult.Results.Count, model.Results.Count);
            Assert.Equal(pagedResult.Results[0].Name, model.Results[0].Name);
        }

        [Fact]
        public async Task Details_Should_Return_NotFound_When_Id_Is_Null()
        {
            int? id = null;
            var result = await controller.Details(id) as NotFoundResult;
            Assert.NotNull(result);
        }

        [Fact]
        public async Task Details_Should_Return_Correct_View_With_Model()
        {
            var beer = new Beer { Id = 1, Name = "Beer 1", AlcoholPercentage = 5.0 };
            beerServiceMock.Setup(x => x.GetBeerByIdAsync(1)).ReturnsAsync(beer);

            var result = await controller.Details(1) as ViewResult;

            Assert.NotNull(result);
            Assert.NotNull(result.Model);
            Assert.IsType<Beer>(result.Model);
            var model = (Beer)result.Model;
            Assert.Equal(beer.Id, model.Id);
            Assert.Equal(beer.Name, model.Name);
            Assert.Equal(beer.AlcoholPercentage, model.AlcoholPercentage);
        }

        [Fact]
        public async Task Create_Should_Return_Correct_View()
        {
            var result = await controller.Create(null) as ViewResult;
            Assert.NotNull(result);
        }

        [Fact]
        public async Task Delete_Should_Return_Correct_View_When_Id_Is_Valid()
        {
            int id = 1;
            var beer = new Beer { Id = id, Name = "Beer 1", AlcoholPercentage = 5.0 };
            beerServiceMock.Setup(x => x.GetBeerByIdAsync(id)).ReturnsAsync(beer);

            var resultRaw = await controller.Delete(id);
            var result = resultRaw as ViewResult;

            Assert.NotNull(result);
            Assert.NotNull(result.Model);
            var model = Assert.IsType<Beer>(result.Model);
            Assert.Equal(beer.Id, model.Id);
            Assert.Equal(beer.Name, model.Name);
            Assert.Equal(beer.AlcoholPercentage, model.AlcoholPercentage);
        }

        [Fact]
        public async Task Delete_Should_Return_NotFound_When_Id_Is_Null()
        {
            int? id = null;
            var result = await controller.Delete(id) as NotFoundResult;
            Assert.NotNull(result);
        }

        [Fact]
        public async Task DeleteConfirmed_Should_Delete_Beer_And_Redirect()
        {
            int id = 1;
            beerServiceMock.Setup(x => x.Delete(id)).Verifiable();

            var result = await controller.DeleteConfirmed(id) as RedirectToActionResult;

            Assert.NotNull(result);
            Assert.Equal("Index", result.ActionName);  // Проверяем, что редирект идет на Index после удаления

            beerServiceMock.Verify(x => x.Delete(id), Times.Once);  // Убедимся, что метод Delete был вызван один раз
        }
    }
}
