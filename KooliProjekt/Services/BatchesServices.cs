using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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

        public async Task<PagedResult<Batch>> GetPagedBatchesAsync(int page, int pageSize, BatchesSearch search)
        {
            var query = _context.Batches.AsQueryable();

            // Применяем фильтры (поиск по ключевым словам и статусу "сделано")
            if (!string.IsNullOrEmpty(search.Keyword))
            {
                query = query.Where(b => b.Description.Contains(search.Keyword) || b.Code.Contains(search.Keyword));
            }
            if (search.Done.HasValue)
            {
                query = query.Where(b => b.Done == search.Done.Value);
            }

            var totalItems = await query.CountAsync();
            var items = await query.Skip((page - 1) * pageSize).Take(pageSize).ToListAsync();

            return new PagedResult<Batch>
            {
                Items = items,
                TotalItems = totalItems,
                Page = page,
                PageSize = pageSize
            };
        }

        public async Task<Batch> Get(int id)
        {
            return await _context.Batches.FindAsync(id);
        }

        public async Task Save(Batch batch)
        {
            if (batch.Id == 0)
            {
                _context.Add(batch);
            }
            else
            {
                _context.Update(batch);
            }
            await _context.SaveChangesAsync();
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
    }
}
