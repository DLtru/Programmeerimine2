using KooliProjekt.Controllers;
using KooliProjekt.Data;
using KooliProjekt.Models;
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
    public class LogEntriesControllerTests
    {
        private readonly Mock<IBeerService> _beerServiceMock;
        private readonly BeersController _controller;

        public LogEntriesControllerTests()
        {
            _beerServiceMock = new Mock<IBeerService>();
            _controller = new BeersController(_beerServiceMock.Object);
        }

        [Fact]
        public async Task Delete_should_return_correct_view_with_model()
        {
            var beer = new Beer { Id = 1, Name = "Test Beer", Type = "Lager" };
            _beerServiceMock.Setup(x => x.GetBeerByIdAsync(1)).ReturnsAsync(beer);

            var result = await _controller.Delete(1) as ViewResult;

            Assert.NotNull(result);
            var model = result.Model as Beer;
            Assert.NotNull(model);
            Assert.Equal(beer.Id, model.Id);
            Assert.Equal(beer.Name, model.Name);
        }

        [Fact]
        public async Task Delete_should_return_notfound_when_beer_not_found()
        {
            _beerServiceMock.Setup(x => x.GetBeerByIdAsync(1)).ReturnsAsync((Beer)null);

            var result = await _controller.Delete(1) as NotFoundResult;

            Assert.NotNull(result);
        }

        [Fact]
        public async Task DeleteConfirmed_should_redirect_to_index_when_beer_is_deleted()
        {
            var beerId = 1;
            _beerServiceMock.Setup(x => x.DeleteBeerAsync(beerId)).Returns(Task.CompletedTask);

            var result = await _controller.DeleteConfirmed(beerId) as RedirectToActionResult;

            Assert.NotNull(result);
            Assert.Equal("Index", result.ActionName);
        }

        [Fact]
        public async Task Details_should_return_correct_view_with_model()
        {
            var beer = new Beer { Id = 1, Name = "Test Beer", Type = "Lager" };
            _beerServiceMock.Setup(x => x.GetBeerByIdAsync(1)).ReturnsAsync(beer);

            var result = await _controller.Details(1) as ViewResult;

            Assert.NotNull(result);
            var model = result.Model as Beer;
            Assert.NotNull(model);
            Assert.Equal(beer.Id, model.Id);
            Assert.Equal(beer.Name, model.Name);
        }

        [Fact]
        public async Task Details_should_return_notfound_when_beer_not_found()
        {
            _beerServiceMock.Setup(x => x.GetBeerByIdAsync(1)).ReturnsAsync((Beer)null);

            var result = await _controller.Details(1) as NotFoundResult;

            Assert.NotNull(result);
        }
    }
}
