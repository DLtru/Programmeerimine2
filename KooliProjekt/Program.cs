using KooliProjekt.Data;
using KooliProjekt.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace KooliProjekt
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Configure the database connection
            var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
            builder.Services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(connectionString));
            builder.Services.AddDatabaseDeveloperPageExceptionFilter();

            // Configure identity
            builder.Services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true)
                .AddEntityFrameworkStores<ApplicationDbContext>();
            builder.Services.AddControllersWithViews();

            // Register services for dependency injection
            builder.Services.AddScoped<IBatchService, BatchService>();
            builder.Services.AddScoped<BeerService, BeerService>();
            builder.Services.AddScoped<ITastingEntryService, TastingEntryService>();
            builder.Services.AddScoped<IPhotoService, PhotoService>();
            builder.Services.AddScoped<ILogEntryService, LogEntryService>();
            builder.Services.AddScoped<ITastingEntryService, TastingEntryService>();

            var app = builder.Build();

            if (app.Environment.IsDevelopment())
            {
                app.UseMigrationsEndPoint();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();

            // Configure route mapping
            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");
            app.MapRazorPages();

            // Ensure database is created and seeded
            using (var scope = app.Services.CreateScope())
            using (var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>())
            using (var userManager = scope.ServiceProvider.GetRequiredService<UserManager<IdentityUser>>())
            {
                context.Database.EnsureCreated();
                SeedData.Generate(context, userManager);
            }

            app.Run();
        }
    }
}
