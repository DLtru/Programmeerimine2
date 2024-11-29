using KooliProjekt.Data;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;


namespace KooliProjekt.Services
{
    public interface ILogEntryService
    {
        Task<IEnumerable<LogEntry>> GetPagedLogEntriesAsync(int page, int pageSize);
        Task<LogEntry> GetLogEntryByIdAsync(int id);
        Task CreateLogEntryAsync(LogEntry logEntry);
        Task UpdateLogEntryAsync(LogEntry logEntry);
        Task DeleteLogEntryAsync(int id);
    }

    namespace KooliProjekt.Services
    {
        public class LogEntryService : ILogEntryService
        {
            private readonly ApplicationDbContext _context;

            public LogEntryService(ApplicationDbContext context)
            {
                _context = context;
            }

            public async Task<IEnumerable<LogEntry>> GetPagedLogEntriesAsync(int page, int pageSize)
            {
                return await _context.LogEntries
                    .Skip((page - 1) * pageSize)
                    .Take(pageSize)
                    .ToListAsync();
            }

            public async Task<LogEntry> GetLogEntryByIdAsync(int id)
            {
                return await _context.LogEntries
                    .Include(l => l.User)
                    .FirstOrDefaultAsync(m => m.Id == id);
            }

            public async Task CreateLogEntryAsync(LogEntry logEntry)
            {
                _context.Add(logEntry);
                await _context.SaveChangesAsync();
            }

            public async Task UpdateLogEntryAsync(LogEntry logEntry)
            {
                _context.Update(logEntry);
                await _context.SaveChangesAsync();
            }

            public async Task DeleteLogEntryAsync(int id)
            {
                var logEntry = await _context.LogEntries.FindAsync(id);
                if (logEntry != null)
                {
                    _context.LogEntries.Remove(logEntry);
                    await _context.SaveChangesAsync();
                }
            }
        }
    }
}
