using KooliProjekt.Data;
using Microsoft.EntityFrameworkCore;

namespace KooliProjekt.Services
{
    public interface IBatchService
    {
        Task<PagedResult<Batch>> GetPagedBatchesAsync(int page, int pageSize, Search.BatchesSearch searchModel);
        Task<Batch> GetBatchByIdAsync(int id);
        Task AddBatchAsync(Batch batch);
        Task UpdateBatchAsync(Batch batch);
        Task DeleteBatchAsync(int id);
    }
}

namespace KooliProjekt.Services
{
    public class BatchService : IBatchService
    {
        private readonly ApplicationDbContext _context;

        public BatchService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<PagedResult<Batch>> GetPagedBatchesAsync(int page, int pageSize, Search.BatchesSearch searchModel)
        {
            var query = _context.Batches.AsQueryable();

            searchModel = searchModel ?? new Search.BatchesSearch();

            if (!string.IsNullOrWhiteSpace(searchModel.Keyword))
            {
                query = query.Where(batch => batch.Code.Contains(searchModel.Keyword) || batch.Description.Contains(searchModel.Keyword));
            }


            if (searchModel.Done != null)
            {
                if (searchModel.Done.Value)
                {
                    query = query.Where(batch => batch.Done);  
                }
                else
                {
                    query = query.Where(batch => !batch.Done);  
                }
            }

            var totalItems = await query.CountAsync();
            var batches = await query
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            var result = new PagedResult<Batch>
            {
                Results = batches,
                TotalCount = totalItems,
                PageIndex = page,
                PageSize = pageSize
            };

            return result;
        }

        public async Task<Batch> GetBatchByIdAsync(int id)
        {
            return await _context.Batches
                .FirstOrDefaultAsync(b => b.Id == id);
        }

        public async Task AddBatchAsync(Batch batch)
        {
            _context.Batches.Add(batch);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateBatchAsync(Batch batch)
        {
            _context.Batches.Update(batch);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteBatchAsync(int id)
        {
            var batch = await _context.Batches.FindAsync(id);
            if (batch != null)
            {
                _context.Batches.Remove(batch);
                await _context.SaveChangesAsync();
            }
        }
    }
}
