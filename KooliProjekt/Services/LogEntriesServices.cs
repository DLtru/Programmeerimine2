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

        public async Task<PagedResult<LogEntry>> List(int page, int pageSize, LogEntriesSearch search)
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

            // Выполняем запрос с пагинацией и сортировкой
            var results = await query
                .OrderByDescending(le => le.Date)
                .GetPagedAsync(page, pageSize);
            return results;
        }
    }
}