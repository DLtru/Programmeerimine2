using System.Collections.Generic;
using System.Threading.Tasks;
using KooliProjekt.Data;
using KooliProjekt.Models;
using KooliProjekt.Search;

namespace KooliProjekt.Services
{
    public interface IBatchService
    {
        Task<PagedResult<Batch>> List(int page, int pageSize, BatchesSearch search);
        Task<Batch> GetById(int id); // Метод для получения Batch по ID
        Task<string> GetBatchByIdAsync(int value); // Метод для получения кода Batch по ID
        Task AddBatchAsync(Batch batch);
        Task Add(Batch batch);
        Task UpdateBatchAsync(Batch batch);
        Task Update(Batch batch);
        Task DeleteBatchAsync(int id);
        Task Delete(int id);
        bool BatchExists(int id);
    }
}
