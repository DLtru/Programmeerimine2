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
    public class PhotoControllerTests
    {
        private readonly Mock<IPhotoService> _PhotoServiceMock;
        private readonly PhotosController _controller;

        public PhotoControllerTests()
        {
            _PhotoServiceMock = new Mock<IPhotoService>();
            _controller = new PhotosController(_PhotoServiceMock.Object);
        }

        [Fact]
        public async Task Index_should_return_correct_view_with_data()
        {
            // Arrange
            int page = 1;
            var data = new List<Photo>
            {
                new Photo { Id = 1, Title = "Test 1" },
                new Photo { Id = 2, Title = "Test 2" }
            };
            var pagedResult = new PagedResult<Photo> { Results = data };
            _PhotoServiceMock.Setup(x => x.List(page, It.IsAny<int>(), null)).ReturnsAsync(pagedResult);

            // Act
            var result = await _controller.Index(page) as ViewResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal(pagedResult, result.Model);
        }
    }
}