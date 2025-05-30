using KooliProjekt.Data;
using Microsoft.EntityFrameworkCore;
using System;

namespace KooliProjekt.UnitTests
{
    public abstract class HomeServiceTestBase : IDisposable
    {
        protected readonly ApplicationDbContext DbContext;

        protected HomeServiceTestBase()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            DbContext = new ApplicationDbContext(options);
        }

        public void Dispose()
        {
            DbContext.Database.EnsureDeleted();
            DbContext.Dispose();
        }
    }
}