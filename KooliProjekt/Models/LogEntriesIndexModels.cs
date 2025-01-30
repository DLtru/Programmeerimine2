using KooliProjekt.Data;
using KooliProjekt.Search;

namespace KooliProjekt.Models
{
    public class LogEntriesIndexModel
    {
        public LogEntriesSearch Search { get; set; }
        public PagedResult<LogEntry> Data { get; set; }
    }
}