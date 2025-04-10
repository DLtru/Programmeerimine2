using KooliProjekt.Data;
using Microsoft.EntityFrameworkCore;
using System;

namespace KooliProjekt.UnitTests.ServiceTests
{
    public abstract class ServiceTestBase : IDisposable
    {
        private ApplicationDbContext _dbContext;
        private bool _disposed;

        protected ApplicationDbContext DbContext
        {
            get
            {
                if (_dbContext == null)
                {
                    var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                        .UseInMemoryDatabase(Guid.NewGuid().ToString())
                        .Options;
                    _dbContext = new ApplicationDbContext(options);
                }

                return _dbContext;
            }
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    _dbContext?.Dispose();
                }

                _disposed = true;
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
