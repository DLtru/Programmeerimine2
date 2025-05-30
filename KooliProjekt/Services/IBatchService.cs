using KooliProjekt.Data;
using KooliProjekt.Models;
using KooliProjekt.Search;
using System.Threading.Tasks;

namespace KooliProjekt.Services
{
    public interface IBatchService
    {
        Task<PagedResult<Batch>> List(int page, int pageSize, BatchesSearch search);
        Task<Batch> Get(int id);
        Task Save(Batch batch);
        Task Delete(int id);
        Task AddBatchAsync(Batch batch);
        Task<string> GetBatchByIdAsync(int value);
        Task UpdateBatchAsync(Batch batch);
        Task<string> GetById(int value);
        bool BatchExists(int id);
    }
}