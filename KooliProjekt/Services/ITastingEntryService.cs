using KooliProjekt.Data;
using KooliProjekt.Models;
using KooliProjekt.Search;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace KooliProjekt.Services
{
    public interface ITastingEntryService
    {
        Task<PagedResult<TastingEntry>> List(int page, int pageSize, TastingEntriesSearch search);
        Task<TastingEntry> GetTastingEntryByIdAsync(int id);
        Task<TastingEntry> AddTastingEntryAsync(TastingEntry entry);
        Task<TastingEntry> UpdateTastingEntryAsync(TastingEntry entry);
        Task DeleteTastingEntryAsync(int id);
        Task<List<BatchSelectListItem>> GetBatches();
        Task<List<UserSelectListItem>> GetUsers();
    }
}