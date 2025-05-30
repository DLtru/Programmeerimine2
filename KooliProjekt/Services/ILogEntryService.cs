using KooliProjekt.Data;
using KooliProjekt.Models;
using KooliProjekt.Search;
using System.Threading.Tasks;

namespace KooliProjekt.Services
{
    public interface ILogEntryService
    {
        Task<PagedResult<LogEntry>> List(int page, int pageSize, LogEntriesSearch search);
        Task<LogEntry> Get(int id);
        Task Save(LogEntry entry);
        Task Delete(int id);
    }
}