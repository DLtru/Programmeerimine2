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
    public class BatchesControllerTests
    {
        private readonly Mock<IBatchService> _BatchesServiceMock;
        private readonly BatchesController _controller;

        public BatchesControllerTests()
        {
            _BatchesServiceMock = new Mock<IBatchService>();
            _controller = new BatchesController(_BatchesServiceMock.Object);
        }

        [Fact]
        public async Task Index_should_return_correct_view_with_data()
        {
            // Arrange
            int page = 1;
            var data = new List<Batch>
            {
                new Batch { Id = 1, Title = "Test 1" },
                new Batch { Id = 2, Title = "Test 2" }
            };
            var pagedResult = new PagedResult<Batch> { Results = data };
            _BatchesServiceMock.Setup(x => x.List(page, It.IsAny<int>(), null)).ReturnsAsync(pagedResult);

            // Act
            var result = await _controller.Index(page) as ViewResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal(pagedResult, result.Model);
        }
    }
}