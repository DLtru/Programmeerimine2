using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using KooliProjekt.Data;
using KooliProjekt.Models;
using KooliProjekt.Search;
using Microsoft.EntityFrameworkCore;

namespace KooliProjekt.Services
{
    public class TastingEntryService : ITastingEntryService
    {
        private readonly ApplicationDbContext _context;

        public TastingEntryService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<TastingEntriesIndexModel> List(int page, int pageSize, TastingEntriesSearch search)
        {
            var query = _context.TastingEntries
                .Include(t => t.Batch)
                .Include(t => t.User)
                .AsQueryable();

            if (!string.IsNullOrEmpty(search.Keyword))
            {
                query = query.Where(t =>
                    t.Comments.Contains(search.Keyword) ||
                    t.Batch.Code.Contains(search.Keyword) ||
                    t.User.Email.Contains(search.Keyword));
            }

            if (search.MinRating.HasValue)
                query = query.Where(t => t.Rating >= search.MinRating.Value);

            if (search.MaxRating.HasValue)
                query = query.Where(t => t.Rating <= search.MaxRating.Value);

            if (search.StartDate.HasValue)
                query = query.Where(t => t.Date >= search.StartDate.Value);

            if (search.EndDate.HasValue)
                query = query.Where(t => t.Date <= search.EndDate.Value);

            var totalCount = await query.CountAsync();
            var results = await query.GetPagedAsync(page, pageSize);

            return new TastingEntriesIndexModel
            {
                Search = search,
                Data = results
            };
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
            var tastingEntry = await GetTastingEntryByIdAsync(id);
            if (tastingEntry == null)
            {
                return;
            }

            _context.TastingEntries.Remove(tastingEntry);
            await _context.SaveChangesAsync();
        }

        public IEnumerable<Batch> GetBatches()
        {
            return _context.Batches.ToList();
        }

        public IEnumerable<User> GetUsers()
        {
            return (IEnumerable<User>)_context.Users.ToList();
        }
    }
}
