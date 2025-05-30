using KooliProjekt.Data;
using KooliProjekt.Models;
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
                query = query.Where(x => x.Description.Contains(search.Keyword));
            }

            if (search.StartDate.HasValue)
            {
                query = query.Where(x => x.Date >= search.StartDate.Value);
            }

            if (search.EndDate.HasValue)
            {
                query = query.Where(x => x.Date <= search.EndDate.Value);
            }

            var totalCount = await query.CountAsync();
            var items = await query
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return new PagedResult<LogEntry>
            {
                Data = new PagedList<LogEntry>
                {
                    Results = items,
                    Total = totalCount
                }
            };
        }

        public async Task<LogEntry> Get(int id)
        {
            return await _context.LogEntries.FindAsync(id);
        }

        public async Task Save(LogEntry entry)
        {
            if (entry.Id == 0)
            {
                _context.LogEntries.Add(entry);
            }
            else
            {
                _context.LogEntries.Update(entry);
            }
            await _context.SaveChangesAsync();
        }

        public async Task Delete(int id)
        {
            var entry = await _context.LogEntries.FindAsync(id);
            if (entry != null)
            {
                _context.LogEntries.Remove(entry);
                await _context.SaveChangesAsync();
            }
        }
    }
}