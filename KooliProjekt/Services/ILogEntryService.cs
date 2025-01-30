using KooliProjekt.Data;
using KooliProjekt.Search;

namespace KooliProjekt.Services
{
    public interface ILogEntryService
    {
        Task<PagedResult<LogEntry>> GetLogEntriesBySearchAsync(LogEntriesSearch search, int page, int pageSize);
    }
}