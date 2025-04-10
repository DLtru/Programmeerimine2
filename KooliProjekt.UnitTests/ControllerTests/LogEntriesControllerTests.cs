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
        public async Task Create_should_return_correct_view_when_model_is_valid()
        {
            var beer = new Beer { Id = 1, Name = "Test Beer", Type = "Lager" };

            _controller.ModelState.Clear();  // Ensure model state is valid

            var result = await _controller.Create(beer) as RedirectToActionResult;

            Assert.NotNull(result);
            Assert.Equal("Index", result.ActionName);
            _beerServiceMock.Verify(x => x.AddBeerAsync(beer), Times.Once);
        }

        [Fact]
        public async Task Create_should_return_view_when_model_is_invalid()
        {
            var beer = new Beer { Id = 1, Name = "", Type = "Lager" };  // Invalid model due to empty name

            _controller.ModelState.AddModelError("Name", "The Name field is required.");

            var result = await _controller.Create(beer) as ViewResult;

            Assert.NotNull(result);
            Assert.Equal(beer, result.Model);
            _beerServiceMock.Verify(x => x.AddBeerAsync(It.IsAny<Beer>()), Times.Never);
        }

        [Fact]
        public async Task Edit_should_return_correct_view_when_model_is_valid()
        {
            var beer = new Beer { Id = 1, Name = "Test Beer", Type = "Lager" };
            _beerServiceMock.Setup(x => x.GetBeerByIdAsync(beer.Id)).ReturnsAsync(beer);

            var result = await _controller.Edit(beer.Id) as ViewResult;

            Assert.NotNull(result);
            var model = result.Model as Beer;
            Assert.NotNull(model);
            Assert.Equal(beer.Id, model.Id);
            Assert.Equal(beer.Name, model.Name);
        }

        [Fact]
        public async Task Edit_should_return_notfound_when_beer_not_found()
        {
            int beerId = 1;
            _beerServiceMock.Setup(x => x.GetBeerByIdAsync(beerId)).ReturnsAsync((Beer)null);

            var result = await _controller.Edit(beerId) as NotFoundResult;

            Assert.NotNull(result);
        }

        [Fact]
        public async Task Edit_should_return_view_when_model_is_invalid()
        {
            var beer = new Beer { Id = 1, Name = "", Type = "Lager" };  // Invalid model due to empty name
            _controller.ModelState.AddModelError("Name", "The Name field is required.");

            var result = await _controller.Edit(beer.Id, beer) as ViewResult;

            Assert.NotNull(result);
            Assert.Equal(beer, result.Model);
            _beerServiceMock.Verify(x => x.UpdateBeerAsync(It.IsAny<Beer>()), Times.Never);
        }

        [Fact]
        public async Task Edit_should_redirect_when_model_is_valid_and_updated()
        {
            var beer = new Beer { Id = 1, Name = "Updated Beer", Type = "Ale" };
            _beerServiceMock.Setup(x => x.UpdateBeerAsync(beer)).Returns(Task.CompletedTask).Verifiable();

            var result = await _controller.Edit(beer.Id, beer) as RedirectToActionResult;

            Assert.NotNull(result);
            Assert.Equal("Index", result.ActionName);
            _beerServiceMock.VerifyAll();
        }

        [Fact]
        public async Task DeleteConfirmed_should_delete_beer_and_redirect()
        {
            var beerId = 1;
            _beerServiceMock.Setup(x => x.Delete(beerId)).Returns(Task.CompletedTask).Verifiable();

            var result = await _controller.DeleteConfirmed(beerId) as RedirectToActionResult;

            Assert.NotNull(result);
            Assert.Equal("Index", result.ActionName);
            _beerServiceMock.VerifyAll();
        }
    }
}
