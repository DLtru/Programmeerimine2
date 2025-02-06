using KooliProjekt.Data;
using KooliProjekt.Search;

namespace KooliProjekt.Services
{
    public interface IBatchService
    {
        Task AddBatchAsync(Batch batch);
        bool BatchExists(int id);
        Task DeleteBatchAsync(int id);
        Task<string> GetBatchByIdAsync(int value);
        Task<PagedResult<Batch>> List(int page, int v, BatchesSearch searchModel);
        Task UpdateBatchAsync(Batch batch);
    }
}