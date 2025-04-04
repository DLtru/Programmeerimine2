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
    public class BatchesControllerTests
    {
        private readonly Mock<IBatchService> batchServiceMock;
        private readonly BatchesController controller;

        public BatchesControllerTests()
        {
            batchServiceMock = new Mock<IBatchService>();
            controller = new BatchesController(batchServiceMock.Object);
        }

        [Fact]
        public async Task Index_Should_Return_Correct_View_With_Data()
        {
            int page = 1;
            var data = new List<Batch>
            {
                new Batch { Id = 1, Code = "B001", Description = "First Batch" },
                new Batch { Id = 2, Code = "B002", Description = "Second Batch" }
            };
            var pagedResult = new PagedResult<Batch>
            {
                Results = data,
                CurrentPage = 1,
                PageCount = 1,
                PageIndex = 1,
                PageNumber = 1
            };
            batchServiceMock.Setup(x => x.List(page, It.IsAny<int>(), It.IsAny<BatchesSearch>())).ReturnsAsync(pagedResult);

            var result = await controller.Index(page) as ViewResult;

            Assert.NotNull(result);
            Assert.NotNull(result.Model);
            Assert.IsType<BatchesIndexModel>(result.Model);
            var model = (BatchesIndexModel)result.Model;
            Assert.NotNull(model.Data);
            Assert.Equal(pagedResult.Results.Count, model.Data.Results.Count);
            Assert.Equal(pagedResult.Results[0].Code, model.Data.Results[0].Code);
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
            var batch = new Batch { Id = 1, Code = "B001", Description = "First Batch" };
            batchServiceMock.Setup(x => x.GetById(1)).ReturnsAsync(batch);

            var result = await controller.Details(1);

            Assert.NotNull(result);
            var viewResult = Assert.IsType<ViewResult>(result);
            Assert.NotNull(viewResult.Model);
            var model = Assert.IsType<Batch>(viewResult.Model);
            Assert.Equal(batch.Id, model.Id);
            Assert.Equal(batch.Code, model.Code);
            Assert.Equal(batch.Description, model.Description);
        }

        [Fact]
        public void Create_Should_Return_Correct_View()
        {
            var result = controller.Create() as ViewResult;

            Assert.NotNull(result);
        }

        [Fact]
        public async Task Delete_Should_Return_Correct_View_When_Id_Is_Valid()
        {
            int id = 1;
            var batch = new Batch { Id = id, Code = "B001", Description = "First Batch" };

            batchServiceMock.Setup(x => x.GetById(id)).ReturnsAsync(batch);

            var resultRaw = await controller.Delete(id);
            var result = resultRaw as ViewResult;

            Assert.NotNull(result);
            Assert.NotNull(result.Model);
            var model = Assert.IsType<Batch>(result.Model);
            Assert.Equal(batch.Id, model.Id);
            Assert.Equal(batch.Code, model.Code);
            Assert.Equal(batch.Description, model.Description);
        }



        [Fact]
        public async Task Delete_Should_Return_NotFound_When_Id_Is_Null()
        {
            int? id = null;

            var result = await controller.Delete(id) as NotFoundResult;

            Assert.NotNull(result);
        }
        [Fact]
        public async Task DeleteConfirmed_Should_Delete_Batch_And_Redirect()
        {
            int id = 1;

            batchServiceMock.Setup(x => x.Delete(id)).Returns(Task.CompletedTask).Verifiable();

            var result = await controller.DeleteConfirmed(id) as RedirectToActionResult;

            Assert.NotNull(result);
            Assert.Equal("Index", result.ActionName);

            batchServiceMock.Verify(x => x.Delete(id), Times.Once);
        }
    }
}