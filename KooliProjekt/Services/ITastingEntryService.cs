using KooliProjekt.Data;
using KooliProjekt.Models;
using KooliProjekt.Search;

namespace KooliProjekt.Services
{
    public interface ITastingEntryService
    {
        Task<TastingEntriesIndexModel> List(int page, int v, TastingEntriesSearch searchModel);
        Task<TastingEntry> GetTastingEntryByIdAsync(int id);
        Task AddTastingEntryAsync(TastingEntry tastingEntry);
        Task UpdateTastingEntryAsync(TastingEntry tastingEntry);
        Task DeleteTastingEntryAsync(int id);
        IEnumerable<Batch> GetBatches();
        IEnumerable<User> GetUsers();
    }
}