using KooliProjekt.Controllers;
using KooliProjekt.Models;
using KooliProjekt.Search;
using KooliProjekt.Services;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using KooliProjekt.Data;

namespace KooliProjekt.UnitTests.ControllerTests
{
    public class BatchesControllerTests
    {
        private readonly Mock<IBatchService> _batchServiceMock;
        private readonly BatchesController _controller;

        public BatchesControllerTests()
        {
            _batchServiceMock = new Mock<IBatchService>();
            _controller = new BatchesController(_batchServiceMock.Object);
        }

        [Fact]
        public async Task Index_Should_Return_Correct_View_With_Data()
        {
            // Arrange
            int page = 1;
            var data = new List<Batch>
            {
                new Batch { Id = 1, Date = DateTime.Now, Code = "Code1", Description = "Test 1" },
                new Batch { Id = 2, Date = DateTime.Now, Code = "Code2", Description = "Test 2" }
            };
            var pagedResult = new PagedResult<Batch>();
            var searchModel = new BatchesSearch { Keyword = null, Done = null };

            _batchServiceMock.Setup(x => x.List(page, 5, It.IsAny<BatchesSearch>()))
                             .ReturnsAsync(pagedResult);

            // Act
            var result = await _controller.Index(page) as ViewResult;

            // Assert
            Assert.NotNull(result);
            var model = result.Model as BatchesIndexModel;
            Assert.NotNull(model);
            Assert.Equal(pagedResult.Results, model.Data.Results);
            Assert.Equal(searchModel.Keyword, model.Search.Keyword);
            Assert.Equal(searchModel.Done, model.Search.Done);
        }

        [Fact]
        public async Task Details_Should_Return_NotFound_When_Id_Is_Null()
        {
            // Arrange
            int? id = null;

            // Act
            var result = await _controller.Details(id) as NotFoundResult;

            // Assert
            Assert.NotNull(result);
        }

        [Fact]
        public async Task Details_Should_Return_NotFound_When_Item_Not_Found()
        {
            // Arrange
            int id = 1;
            _batchServiceMock.Setup(x => x.GetById(id));

            // Act
            var result = await _controller.Details(id) as NotFoundResult;

            // Assert
            Assert.NotNull(result);
        }

        [Fact]
        public async Task Details_Should_Return_Correct_View_With_Model_When_Item_Found()
        {
            // Arrange
            int id = 1;
            var batch = new Batch { Id = id, Code = "Code1", Description = "Test Batch" };
            _batchServiceMock.Setup(x => x.GetById(id)).ReturnsAsync(batch);

            // Act
            var result = await _controller.Details(id) as ViewResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal(batch, result.Model);
        }

        [Fact]
        public void Create_Should_Return_Correct_View()
        {
            // Act
            var result = _controller.Create() as ViewResult;

            // Assert
            Assert.NotNull(result);
            Assert.True(string.IsNullOrEmpty(result.ViewName) || result.ViewName == "Create");
        }

        [Fact]
        public async Task Delete_Should_Return_NotFound_When_Id_Is_Null()
        {
            // Arrange
            int? id = null;

            // Act
            var result = await _controller.Delete(id) as NotFoundResult;

            // Assert
            Assert.NotNull(result);
        }

        [Fact]
        public async Task Delete_Should_Return_NotFound_When_Item_Not_Found()
        {
            // Arrange
            int id = 1;
            _batchServiceMock.Setup(x => x.GetById(id)).ReturnsAsync((Batch)null);

            // Act
            var result = await _controller.Delete(id) as NotFoundResult;

            // Assert
            Assert.NotNull(result);
        }

        [Fact]
        public async Task Delete_Should_Return_Correct_View_With_Model_When_Item_Found()
        {
            // Arrange
            int id = 1;
            var batch = new Batch { Id = id, Code = "Code1", Description = "Test Batch" };
            _batchServiceMock.Setup(x => x.List(id, id, null));

            // Act
            var result = await _controller.Delete(id) as ViewResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal(batch, result.Model);
        }
    }
}