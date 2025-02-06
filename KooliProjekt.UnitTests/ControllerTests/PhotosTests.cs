using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using KooliProjekt.Controllers;
using KooliProjekt.Services;
using KooliProjekt.Models;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;
using KooliProjekt.Data;
using KooliProjekt.Search;

namespace KooliProjekt.UnitTests.ControllerTests
{
    public class PhotoControllerTests
    {
        private readonly Mock<IPhotoService> _photoServiceMock;
        private readonly PhotosController _controller;

        public PhotoControllerTests()
        {
            _photoServiceMock = new Mock<IPhotoService>();
            _controller = new PhotosController(_photoServiceMock.Object);
        }

        [Theory]
        [InlineData(1)] // Пример передачи значения для pageIndex
        public async Task IndexShouldReturnCorrectViewWithData(int pageIndex)
        {
            // Arrange
            var data = new List<Photo>
            {
                new Photo { Id = 1, Title = "Test 1" },
                new Photo { Id = 2, Title = "Test 2" }
            };

            var pagedResult = new PagedResult<Photo>
            {
                Results = data,
                CurrentPage = pageIndex,
                PageCount = 1
            };

            _photoServiceMock.Setup(x => x.List(pageIndex, It.IsAny<int>(), It.IsAny<PhotosSearch>()))
                .ReturnsAsync(pagedResult)
                .Verifiable();

            // Act
            var result = await _controller.Index(pageIndex) as ViewResult;

            // Проверка, что результат контроллера не null
            Assert.NotNull(result);

            // Проверка, что результат — это ViewResult
            Assert.IsType<ViewResult>(result);

            // Проверка, что модель в результате не null
            Assert.NotNull(result.Model);

            // Проверка, что модель является PagedResult<Photo>
            var model = Assert.IsType<PagedResult<Photo>>(result.Model);

            // Проверка, что количество элементов в модели совпадает с количеством в data
            Assert.Equal(data.Count, model.Results.Count);

            // Убедимся, что сервис был вызван один раз
            _photoServiceMock.VerifyAll();
        }

        [Fact]
        public void Create_should_return_view_on_get()
        {
            // Arrange & Act
            var result = _controller.Create() as ViewResult;

            // Assert
            Assert.NotNull(result);
        }

        [Fact]
        public async Task Create_should_redirect_when_valid_model()
        {
            // Arrange
            var photo = new Photo { Title = "New Photo" };
            _photoServiceMock.Setup(x => x.CreatePhotoAsync(photo)).Returns(Task.CompletedTask);

            // Act
            var result = await _controller.Create(photo) as RedirectToActionResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal("Index", result.ActionName);
        }

        [Fact]
        public async Task Edit_should_return_view_when_photo_found()
        {
            // Arrange
            var photo = new Photo { Id = 1, Title = "Test Photo" };
            _photoServiceMock.Setup(x => x.GetPhotoByIdAsync(1)).ReturnsAsync(photo);

            // Act
            var result = await _controller.Edit(1) as ViewResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal(photo, result.Model);
        }

        [Fact]
        public async Task DeleteConfirmed_should_redirect_on_success()
        {
            // Arrange
            int photoId = 1;
            _photoServiceMock.Setup(x => x.DeletePhotoAsync(photoId)).Returns(Task.CompletedTask);

            // Act
            var result = await _controller.DeleteConfirmed(photoId) as RedirectToActionResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal("Index", result.ActionName);
        }
    }
}
