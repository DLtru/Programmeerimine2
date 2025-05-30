using KooliProjekt.Data;
using KooliProjekt.Models;
using KooliProjekt.Search;
using Microsoft.EntityFrameworkCore;
using System.Collections;
using System.Linq;
using System.Threading.Tasks;

namespace KooliProjekt.Services
{
    public class TastingEntryService : ITastingEntryService
    {
        private readonly ApplicationDbContext _context;

        public TastingEntryService(ApplicationDbContext context)
        {
            _context = context;
        }

        // ... другие методы остаются без изменений ...

        public async Task<List<BatchSelectListItem>> GetBatches()
        {
            return await _context.Batches
                .Select(b => new BatchSelectListItem
                {
                    Id = b.Id,
                    Code = b.Code
                })
                .ToListAsync();
        }

        public async Task<List<UserSelectListItem>> GetUsers()
        {
            return await _context.TastingEntries
                .Select(t => new UserSelectListItem
                {
                    Id = t.UserId,
                    Name = t.UserId
                })
                .Distinct()
                .ToListAsync();
        }
    }