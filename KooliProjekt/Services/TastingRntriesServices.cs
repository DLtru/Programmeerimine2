using KooliProjekt.Data;
using KooliProjekt.Services;
using Microsoft.EntityFrameworkCore;
using KooliProjekt.Models;

namespace KooliProjekt.Services
{
    public interface ITastingEntryService
    {
        Task<List<TastingEntry>> GetPagedTastingEntriesAsync(int page, int pageSize);
        Task<TastingEntry> GetTastingEntryByIdAsync(int id);
        Task AddTastingEntryAsync(TastingEntry tastingEntry);
        Task UpdateTastingEntryAsync(TastingEntry tastingEntry);
        Task DeleteTastingEntryAsync(int id);
    }
}

namespace KooliProjekt.Services
{
    public class TastingEntryService : ITastingEntryService
    {
        private readonly ApplicationDbContext _context;

        public TastingEntryService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<TastingEntry>> GetPagedTastingEntriesAsync(int page, int pageSize)
        {
            return await _context.TastingEntries
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .Include(t => t.Batch)
                .Include(t => t.User)
                .ToListAsync();
        }

        public async Task<TastingEntry> GetTastingEntryByIdAsync(int id)
        {
            return await _context.TastingEntries
                .Include(t => t.Batch)
                .Include(t => t.User)
                .FirstOrDefaultAsync(t => t.Id == id);
        }

        public async Task AddTastingEntryAsync(TastingEntry tastingEntry)
        {
            _context.TastingEntries.Add(tastingEntry);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateTastingEntryAsync(TastingEntry tastingEntry)
        {
            _context.TastingEntries.Update(tastingEntry);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteTastingEntryAsync(int id)
        {
            var tastingEntry = await _context.TastingEntries.FindAsync(id);
            if (tastingEntry != null)
            {
                _context.TastingEntries.Remove(tastingEntry);
                await _context.SaveChangesAsync();
            }
        }
    }
}
