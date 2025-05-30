using KooliProjekt.Data;
using KooliProjekt.Models;
using KooliProjekt.Search;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

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
                query = query.Where(x => x.Description.Contains(search.Keyword));
            }

            if (!string.IsNullOrEmpty(search.Code))
            {
                query = query.Where(x => x.Code.Contains(search.Code));
            }

            if (search.StartDate.HasValue)
            {
                query = query.Where(x => x.Date >= search.StartDate.Value);
            }

            if (search.EndDate.HasValue)
            {
                query = query.Where(x => x.Date <= search.EndDate.Value);
            }

            var totalCount = await query.CountAsync();
            var items = await query
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return new PagedResult<Batch>
            {
                Data = new PagedList<Batch>
                {
                    Results = items,
                    Total = totalCount
                }
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
                _context.Batches.Add(batch);
            }
            else
            {
                _context.Batches.Update(batch);
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

        public Task AddBatchAsync(Batch batch)
        {
            throw new NotImplementedException();
        }

        public Task<string> GetBatchByIdAsync(int value)
        {
            throw new NotImplementedException();
        }

        public Task UpdateBatchAsync(Batch batch)
        {
            throw new NotImplementedException();
        }

        public Task<string> GetById(int value)
        {
            throw new NotImplementedException();
        }

        public bool BatchExists(int id)
        {
            throw new NotImplementedException();
        }
    }
}