using KooliProjekt.Data;
using Microsoft.EntityFrameworkCore;

namespace KooliProjekt.Services
{
    public class HomeService
    {
        private readonly ApplicationDbContext dbContext;

        public HomeService(ApplicationDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public List<string> GetHomePageData()
        {
            return new List<string> { "ASP.NET Core", "C#", "MVC", "Razor Pages", "Web API" };
        }
    }
}