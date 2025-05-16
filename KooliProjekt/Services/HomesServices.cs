using KooliProjekt.Data;

namespace KooliProjekt.Services
{
    public class HomeService
    {
        private ApplicationDbContext dbContext;

        public HomeService(ApplicationDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task GetDashboardData()
        {
            throw new NotImplementedException();
        }

        public List<string> GetHomePageData()
        {
            return new List<string> { "ASP.NET Core", "C#", "MVC", "Razor Pages", "Web API" };
        }
    }
}
