using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using KooliProjekt.Controllers;
using KooliProjekt.Models;
using KooliProjekt.Services;
using KooliProjekt.Search;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;
using KooliProjekt.Data;

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

    }
}
