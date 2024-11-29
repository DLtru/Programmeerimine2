using KooliProjekt.Data;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using KooliProjekt.Services;

public class Programs
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Andmebaasi kontekst
        builder.Services.AddDbContext<ApplicationDbContext>(options =>
            options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

        // Teenused
        builder.Services.AddScoped<IPhotoService, PhotoService>();

        var app = builder.Build();

        // Middleware ja lõpp-punktide määramine
        app.MapControllers();

        app.Run();
    }
}


namespace KooliProjekt.Services
{
    public interface IPhotoService
    {
        Task<IEnumerable<Photo>> GetPagedPhotosAsync(int page, int pageSize);
        Task<Photo> GetPhotoByIdAsync(int id);
        Task CreatePhotoAsync(Photo photo);
        Task UpdatePhotoAsync(Photo photo);
        Task DeletePhotoAsync(int id);
    }
}

namespace KooliProjekt.Services
{
    public class PhotoService : IPhotoService
    {
        private readonly ApplicationDbContext _context;

        public PhotoService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Photo>> GetPagedPhotosAsync(int page, int pageSize)
        {
            return await _context.Photos
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
        }

        public async Task<Photo> GetPhotoByIdAsync(int id)
        {
            return await _context.Photos
                .FirstOrDefaultAsync(m => m.Id == id);
        }

        public async Task CreatePhotoAsync(Photo photo)
        {
            _context.Add(photo);
            await _context.SaveChangesAsync();
        }

        public async Task UpdatePhotoAsync(Photo photo)
        {
            _context.Update(photo);
            await _context.SaveChangesAsync();
        }

        public async Task DeletePhotoAsync(int id)
        {
            var photo = await _context.Photos.FindAsync(id);
            if (photo != null)
            {
                _context.Photos.Remove(photo);
                await _context.SaveChangesAsync();
            }
        }
    }
}
