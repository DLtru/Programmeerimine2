using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using KooliProjekt.Controllers;
using KooliProjekt.Data;
using KooliProjekt.Services;
using KooliProjekt.Search;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

namespace KooliProjekt.UnitTests.ControllerTests
{
    public class BeersControllerTests
    {
        private readonly Mock<IBeerService> _beerServiceMock;
        private readonly BeersController _controller;

        public BeersControllerTests()
        {
            _beerServiceMock = new Mock<IBeerService>();
            _controller = new BeersController(_beerServiceMock.Object);
        }

        [Fact]
        public async Task Index_ShouldReturnCorrectViewWithData()
        {
            // Arrange
            int page = 1;
            var searchName = "Test";
            var searchType = "IPA";
            var data = new List<Beer>
            {
                new Beer { Id = 1, Name = "Test Beer 1", Type = "IPA" },
                new Beer { Id = 2, Name = "Test Beer 2", Type = "IPA" }
            };

            var pagedResult = new PagedResult<Beer> { Results = data };

            // Настроим мок-сервис так, чтобы он возвращал данные
            _beerServiceMock.Setup(x => x.List(page, 5, It.IsAny<BeerSearch>()))
                            .ReturnsAsync(pagedResult);

            // Act
            var result = await _controller.Index(page, searchName, searchType) as ViewResult;

            // Assert
            Assert.NotNull(result);  // Проверка, что результат не null
            Assert.NotNull(result.Model);  // Проверка, что модель не null
            Assert.IsType<PagedResult<Beer>>(result.Model);  // Проверка, что модель правильного типа
            var model = (PagedResult<Beer>)result.Model;
            Assert.Equal(pagedResult.Results.Count, model.Results.Count);  // Проверка, что количество данных совпадает

            // Логируем что-то полезное, чтобы увидеть, что передается в тест
            Console.WriteLine($"Returned model contains {model.Results.Count} beers.");
        }
    }
}
