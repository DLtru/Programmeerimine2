using KooliProjekt.Data;
using KooliProjekt.Services;
using KooliProjekt.UnitTests.ServiceTestBase;
using System;
using System.Threading.Tasks;
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

        // Примечание: в зависимости от реализации HomeService, 
        // может потребоваться дополнить или изменить эти тесты

        [Fact]
        public async Task GetDashboardData_should_return_dashboard_data()
        {
            // Arrange
            // Добавьте необходимые данные в контекст базы данных

            // Act
            var result = await _service.GetDashboardData();

            // Assert
            Assert.NotNull(result);
            // Дополнительные утверждения в зависимости от структуры данных Dashboard
        }
    }
}