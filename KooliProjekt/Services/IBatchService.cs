using KooliProjekt.Data;
using KooliProjekt.Search;

namespace KooliProjekt.Services
{
    public interface IBatchService
    {
        Task<PagedResult<Batch>> GetPagedBatchesAsync(int page, int v, BatchesSearch searchModel);
    }
}