using KooliProjekt.Services;
using System.Collections.Generic;
using Xunit;

namespace KooliProjekt.UnitTests.ServiceTests
{
    public class HomeServiceTests : ServiceTestBase
    {
        private readonly HomeService _service;

        public HomeServiceTests()
        {
            _service = new HomeService(DbContext);
        }

        [Fact]
        public void GetHomePageData_ShouldReturnCorrectData()
        {
            // Act
            var result = _service.GetHomePageData();

            // Assert
            Assert.NotNull(result);
            Assert.IsType<List<string>>(result);
            Assert.Equal(5, result.Count);
            Assert.Contains("ASP.NET Core", result);
            Assert.Contains("C#", result);
            Assert.Contains("MVC", result);
            Assert.Contains("Razor Pages", result);
            Assert.Contains("Web API", result);
        }
    }
}