using KooliProjekt.Controllers;
using KooliProjekt.Data;
using KooliProjekt.Services;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

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

        [Fact]
        public async Task Create_should_redirect_when_valid_model()
        {
            var photo = new Photo { Title = "New Photo" };
            _photoServiceMock.Setup(x => x.CreatePhotoAsync(photo)).Returns(Task.CompletedTask);

            var result = await _controller.Create(photo) as RedirectToActionResult;

            Assert.NotNull(result);
            Assert.Equal("Index", result.ActionName);
        }

        [Fact]
        public async Task Create_should_return_view_when_invalid_model()
        {
            var photo = new Photo { Title = "" };  // Invalid model
            _controller.ModelState.AddModelError("Title", "The Title field is required.");

            var result = await _controller.Create(photo) as ViewResult;

            Assert.NotNull(result);
            Assert.Equal(photo, result.Model);
            _photoServiceMock.Verify(x => x.CreatePhotoAsync(It.IsAny<Photo>()), Times.Never);
        }

        [Fact]
        public async Task Edit_should_redirect_when_valid_model()
        {
            var photo = new Photo { Id = 1, Title = "Updated Photo" };
            _photoServiceMock.Setup(x => x.UpdatePhotoAsync(photo)).Returns(Task.CompletedTask);

            var result = await _controller.Edit(photo.Id, photo) as RedirectToActionResult;

            Assert.NotNull(result);
            Assert.Equal("Index", result.ActionName);
        }

        [Fact]
        public async Task Edit_should_return_view_when_invalid_model()
        {
            var photo = new Photo { Id = 1, Title = "" };  // Invalid title
            _controller.ModelState.AddModelError("Title", "The Title field is required.");

            var result = await _controller.Edit(photo.Id, photo) as ViewResult;

            Assert.NotNull(result);
            Assert.Equal(photo, result.Model);
            _photoServiceMock.Verify(x => x.UpdatePhotoAsync(It.IsAny<Photo>()), Times.Never);
        }

        [Fact]
        public async Task DeleteConfirmed_should_redirect_on_success()
        {
            int photoId = 1;
            var photo = new Photo { Id = photoId, Title = "Photo to delete" };
            _photoServiceMock.Setup(x => x.GetPhotoByIdAsync(photoId)).ReturnsAsync(photo);
            _photoServiceMock.Setup(x => x.DeletePhotoAsync(photoId)).Returns(Task.CompletedTask);

            var result = await _controller.DeleteConfirmed(photoId) as RedirectToActionResult;

            Assert.NotNull(result);
            Assert.Equal("Index", result.ActionName);
            _photoServiceMock.VerifyAll();
        }


        [Fact]
        public async Task DeleteConfirmed_should_return_notfound_when_delete_fails()
        {
            int photoId = 1;

            _photoServiceMock.Setup(x => x.GetPhotoByIdAsync(photoId)).ReturnsAsync((Photo) null);

            var result = await _controller.DeleteConfirmed(photoId) as NotFoundResult;

            Assert.NotNull(result);
            Assert.IsType<NotFoundResult>(result);
            _photoServiceMock.Verify(x => x.GetPhotoByIdAsync(photoId), Times.Once);
        }
    }
}
