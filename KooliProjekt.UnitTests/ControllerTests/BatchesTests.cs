using KooliProjekt.Controllers;
using KooliProjekt.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace KooliProjekt.UnitTests.ControllerTests
{
    public class HomeControllerTests
    {
        private readonly HomeController _controller;

        public HomeControllerTests()
        {
            var loggerMock = new Mock<ILogger<HomeController>>();
            _controller = new HomeController(loggerMock.Object);

            var httpContextMock = new Mock<HttpContext>();
            httpContextMock.Setup(x => x.TraceIdentifier).Returns("TestTraceId");
            _controller.ControllerContext = new ControllerContext
            {
                HttpContext = httpContextMock.Object
            };
        }

        [Fact]
        public void Index_Should_Return_Index_View()
        {
            // Act
            var result = _controller.Index() as ViewResult;

            // Assert
            Assert.NotNull(result);
            Assert.True(result.ViewName == "Index" || string.IsNullOrEmpty(result.ViewName));
        }

        [Fact]
        public void Privacy_Should_Return_Correct_View()
        {
            // Act
            var result = _controller.Privacy() as ViewResult;

            // Assert
            Assert.NotNull(result);
            Assert.True(result.ViewName == "Privacy" || string.IsNullOrEmpty(result.ViewName));
        }

        [Fact]
        public void Error_Should_Return_Correct_View()
        {
            // Act
            var result = _controller.Error() as ViewResult;

            // Assert
            Assert.NotNull(result);
            var model = result.Model as ErrorViewModel;
            Assert.NotNull(model);
            Assert.Equal("TestTraceId", model.RequestId);
        }
    }
}
