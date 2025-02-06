using KooliProjekt.Controllers;
using KooliProjekt.Data;
using KooliProjekt.Models;
using KooliProjekt.Search;
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
        private readonly int TotalCount;

        public BatchesControllerTests()
        {
            _BatchesServiceMock = new Mock<IBatchService>();
            _controller = new BatchesController(_BatchesServiceMock.Object);
        }

        public int GetTotalCount()
        {
            return TotalCount;
        }

        [Fact]
        public async Task IndexShouldReturnCorrectViewWithData()
        {
            // Arrange
            int page = 1;
            var data = new List<Batch>
            {
                new Batch { Id = 1, Date = DateTime.Now, Code = "Code1", Description = "Test 1" },
                new Batch { Id = 2, Date = DateTime.Now, Code = "Code2", Description = "Test 2" }
            };

            var pagedResult = new PagedResult<Batch>
            {
                Results = data,
                totalCount = data.Count, // Добавляем total count, если это требуется для пагинации
                PageNumber = page,
                PageSize = 5
            };

            var searchModel = new BatchesSearch
            {
                Keyword = null, // Или добавь свои данные для поиска
                Done = null
            };

            // Мокаем метод List в сервисе
            _BatchesServiceMock.Setup(x => x.List(page, 5, It.IsAny<BatchesSearch>()))
                               .ReturnsAsync(pagedResult);

            // Act
            var result = await _controller.Index(page) as ViewResult;

            // Assert
            Assert.NotNull(result); // Проверяем, что результат не null
            var model = result.Model as BatchesIndexModel;
            Assert.NotNull(model); // Проверяем, что модель возвращена корректно

            // Проверяем правильность данных в модели
            Assert.Equal(pagedResult, model.Data); // Проверяем, что данные в модели совпадают с ожиданием

            // Сравниваем свойства вручную
            Assert.Equal(searchModel.Keyword, model.Search.Keyword);
            Assert.Equal(searchModel.Done, model.Search.Done);
        }
    }
}
