using KooliProjekt.Data;
using KooliProjekt.Models;
using KooliProjekt.Search;
using Microsoft.EntityFrameworkCore;

namespace KooliProjekt.Services
{
    public class PhotoService : IPhotoService
    {
        private readonly ApplicationDbContext _context;

        public PhotoService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Data.Photo> GetPhotoByIdAsync(int id)
        {
            return await _context.Photos
                .FirstOrDefaultAsync(m => m.Id == id);
        }

        public async Task CreatePhotoAsync(Data.Photo photo)
        {
            _context.Add(photo);
            await _context.SaveChangesAsync();
        }

        public async Task UpdatePhotoAsync(Data.Photo photo)
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

        public async Task<PagedResult<Photo>> List(int page, int pageSize, PhotosSearch search)
        {
            var query = _context.Photos.AsQueryable();

            if (!string.IsNullOrEmpty(search.Title))
            {
                query = query.Where(p => p.Description.Contains(search.Title)); // Фильтрация по Description
            }

            if (search.StartDate.HasValue)
            {
                query = query.Where(p => p.Date >= search.StartDate.Value);
            }

            if (search.EndDate.HasValue)
            {
                query = query.Where(p => p.Date <= search.EndDate.Value);
            }

            return await query.GetPagedAsync(page, pageSize);
        }

    }
}
