using KooliProjekt.Data;
using KooliProjekt.Search;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace KooliProjekt.Services
{
    public class LogEntryService : ILogEntryService
    {
        private readonly ApplicationDbContext _context;

        public LogEntryService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<PagedResult<LogEntry>> GetLogEntriesBySearchAsync(LogEntriesSearch search, int page, int pageSize)
        {
            var query = _context.LogEntries.AsQueryable();

            if (!string.IsNullOrEmpty(search.Keyword))
            {
                query = query.Where(le => le.Description.Contains(search.Keyword));
            }

            if (search.StartDate.HasValue)
            {
                query = query.Where(le => le.Date >= search.StartDate.Value);
            }

            if (search.EndDate.HasValue)
            {
                query = query.Where(le => le.Date <= search.EndDate.Value);
            }

            var totalItems = await query.CountAsync();
            var results = await query
                .OrderByDescending(le => le.Date)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return new PagedResult<LogEntry>
            {
                Results = results,
                TotalItems = totalItems,
                Page = page,
                PageSize = pageSize
            };
        }

    }
}
