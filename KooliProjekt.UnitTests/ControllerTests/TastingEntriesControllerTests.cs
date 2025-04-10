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
using System.Threading.Tasks;
using Xunit;

namespace KooliProjekt.UnitTests.ControllerTests
{
    public class TastingEntriesControllerTests
    {
        private readonly Mock<ITastingEntryService> _tastingEntryServiceMock;
        private readonly TastingEntriesController _controller;

        public TastingEntriesControllerTests()
        {
            _tastingEntryServiceMock = new Mock<ITastingEntryService>();
            _controller = new TastingEntriesController(_tastingEntryServiceMock.Object);
        }

        [Fact]
        public async Task Index_should_return_correct_view_with_data()
        {
            int page = 1;
            var data = new List<TastingEntry>
            {
                new TastingEntry { Id = 1, Title = "Test 1" },
                new TastingEntry { Id = 2, Title = "Test 2" }
            };

            var pagedResult = new PagedResult<TastingEntry> { Results = data };
            var model = new TastingEntriesIndexModel { Data = pagedResult };

            _tastingEntryServiceMock.Setup(x => x.List(page, It.IsAny<int>(), null)).ReturnsAsync(model);

            var result = await _controller.Index(null, page) as ViewResult;

            Assert.NotNull(result);
            var viewModel = Assert.IsType<TastingEntriesIndexModel>(result.Model);
            Assert.Equal(data.Count, viewModel.Data.Results.Count);
        }

        [Fact]
        public async Task Delete_should_return_correct_view_with_model()
        {
            var tastingEntry = new TastingEntry { Id = 1, Title = "Test Tasting Entry" };
            _tastingEntryServiceMock.Setup(x => x.GetTastingEntryByIdAsync(1)).ReturnsAsync(tastingEntry);

            var result = await _controller.Delete(1) as ViewResult;

            Assert.NotNull(result);
            var model = result.Model as TastingEntry;
            Assert.NotNull(model);
            Assert.Equal(tastingEntry.Id, model.Id);
            Assert.Equal(tastingEntry.Title, model.Title);
        }

        [Fact]
        public async Task Delete_should_return_notfound_when_tasting_entry_not_found()
        {
            _tastingEntryServiceMock.Setup(x => x.GetTastingEntryByIdAsync(1)).ReturnsAsync((TastingEntry)null);

            var result = await _controller.Delete(1) as NotFoundResult;

            Assert.NotNull(result);
        }

        [Fact]
        public async Task DeleteConfirmed_should_redirect_to_index_when_tasting_entry_is_deleted()
        {
            var tastingEntryId = 1;
            var tastingEntry = new TastingEntry { Id = tastingEntryId, Title = "Test Tasting Entry" };
            _tastingEntryServiceMock.Setup(x => x.GetTastingEntryByIdAsync(tastingEntryId)).ReturnsAsync(tastingEntry);
            _tastingEntryServiceMock.Setup(x => x.DeleteTastingEntryAsync(tastingEntryId)).Returns(Task.CompletedTask);

            var result = await _controller.DeleteConfirmed(tastingEntryId) as RedirectToActionResult;

            Assert.NotNull(result);
            Assert.Equal("Index", result.ActionName);
        }

        [Fact]
        public async Task DeleteConfirmed_should_return_notfound_when_tasting_entry_not_found()
        {
            var tastingEntryId = 1;
            _tastingEntryServiceMock.Setup(x => x.GetTastingEntryByIdAsync(tastingEntryId)).ReturnsAsync((TastingEntry)null);

            var result = await _controller.DeleteConfirmed(tastingEntryId) as NotFoundResult;

            Assert.NotNull(result);
        }

        [Fact]
        public async Task Details_should_return_correct_view_with_model()
        {
            var tastingEntry = new TastingEntry { Id = 1, Title = "Test Tasting Entry" };
            _tastingEntryServiceMock.Setup(x => x.GetTastingEntryByIdAsync(1)).ReturnsAsync(tastingEntry);

            var result = await _controller.Details(1) as ViewResult;

            Assert.NotNull(result);
            var model = result.Model as TastingEntry;
            Assert.NotNull(model);
            Assert.Equal(tastingEntry.Id, model.Id);
            Assert.Equal(tastingEntry.Title, model.Title);
        }

        [Fact]
        public async Task Details_should_return_notfound_when_tasting_entry_not_found()
        {
            _tastingEntryServiceMock.Setup(x => x.GetTastingEntryByIdAsync(1)).ReturnsAsync((TastingEntry)null);

            var result = await _controller.Details(1) as NotFoundResult;

            Assert.NotNull(result);
        }

        [Fact]
        public async Task Edit_should_return_correct_view_when_tasting_entry_found()
        {
            var tastingEntry = new TastingEntry { Id = 1, Title = "Test Tasting Entry" };
            _tastingEntryServiceMock.Setup(x => x.GetTastingEntryByIdAsync(1)).ReturnsAsync(tastingEntry);

            var result = await _controller.Edit(1) as ViewResult;

            Assert.NotNull(result);
            var model = result.Model as TastingEntry;
            Assert.NotNull(model);
            Assert.Equal(tastingEntry.Id, model.Id);
            Assert.Equal(tastingEntry.Title, model.Title);
        }

        [Fact]
        public async Task Edit_should_return_notfound_when_tasting_entry_not_found()
        {
            _tastingEntryServiceMock.Setup(x => x.GetTastingEntryByIdAsync(1)).ReturnsAsync((TastingEntry)null);

            var result = await _controller.Edit(1) as NotFoundResult;

            Assert.NotNull(result);
        }

        [Fact]
        public async Task Edit_should_redirect_when_valid_model_is_updated()
        {
            var tastingEntry = new TastingEntry { Id = 1, Title = "Updated Tasting Entry" };
            _tastingEntryServiceMock.Setup(x => x.UpdateTastingEntryAsync(tastingEntry)).Returns(Task.CompletedTask);

            var result = await _controller.Edit(tastingEntry.Id, tastingEntry) as RedirectToActionResult;

            Assert.NotNull(result);
            Assert.Equal("Index", result.ActionName);
        }

        [Fact]
        public async Task Edit_should_return_view_when_invalid_model()
        {
            var tastingEntry = new TastingEntry { Id = 1, Title = "" };
            _controller.ModelState.AddModelError("Title", "The Title field is required.");

            var result = await _controller.Edit(tastingEntry.Id, tastingEntry) as ViewResult;

            Assert.NotNull(result);
            Assert.Equal(tastingEntry, result.Model);
            _tastingEntryServiceMock.Verify(x => x.UpdateTastingEntryAsync(It.IsAny<TastingEntry>()), Times.Never);
        }
    }
}
