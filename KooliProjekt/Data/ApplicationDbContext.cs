using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace KooliProjekt.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Batch> Batches { get; set; }
        public DbSet<Beer> Beers { get; set; }
        public DbSet<Ingredient> Ingredients { get; set; }
        public DbSet<Photo> Photos { get; set; }
        public DbSet<LogEntry> LogEntries { get; set; }
        public DbSet<TastingEntry> TastingEntries { get; set; }
    }
}