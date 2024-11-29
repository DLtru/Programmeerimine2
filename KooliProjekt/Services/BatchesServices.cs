using KooliProjekt.Data;
using Microsoft.EntityFrameworkCore;
using KooliProjekt.Models;
using KooliProjekt.Services;


namespace KooliProjekt.Services
{
    public interface IBatchService
    {
        Task<List<Batch>> GetPagedBatchesAsync(int page, int pageSize);
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

        public async Task<List<Batch>> GetPagedBatchesAsync(int page, int pageSize)
        {
            return await _context.Batches
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
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
