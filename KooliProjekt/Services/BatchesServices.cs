using System;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using KooliProjekt.Data;
using KooliProjekt.Models;
using KooliProjekt.Search;

namespace KooliProjekt.Services
{
    public class BatchService : IBatchService
    {
        private readonly ApplicationDbContext _context;

        public BatchService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<PagedResult<Batch>> List(int page, int pageSize, BatchesSearch search)
        {
            var query = _context.Batches.AsQueryable();

            if (!string.IsNullOrEmpty(search.Keyword))
            {
                query = query.Where(b => b.Code.Contains(search.Keyword) || b.Description.Contains(search.Keyword));
            }

            if (search.Done.HasValue)
            {
                query = query.Where(b => b.Done == search.Done.Value);
            }

            return await query.GetPagedAsync(page, pageSize);
        }

        public async Task<Batch> GetById(int id)
        {
            return await _context.Batches.FindAsync(id);
        }

        public async Task<string> GetBatchByIdAsync(int value)
        {
            var batch = await _context.Batches.FindAsync(value);
            return batch != null ? batch.Code : null; // Возвращаем только код, если найден
        }

        public async Task Add(Batch batch)
        {
            await _context.Batches.AddAsync(batch);
            await _context.SaveChangesAsync();
        }

        public async Task AddBatchAsync(Batch batch)
        {
            await Add(batch);
        }

        public async Task Update(Batch batch)
        {
            _context.Batches.Update(batch);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateBatchAsync(Batch batch)
        {
            await Update(batch);
        }

        public async Task Delete(int id)
        {
            var batch = await _context.Batches.FindAsync(id);
            if (batch != null)
            {
                _context.Batches.Remove(batch);
                await _context.SaveChangesAsync();
            }
        }

        public async Task DeleteBatchAsync(int id)
        {
            await Delete(id);
        }

        public bool BatchExists(int id)
        {
            return _context.Batches.Any(e => e.Id == id);
        }
    }
}
