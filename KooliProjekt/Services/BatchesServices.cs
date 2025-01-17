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

        public async Task<PagedResult<Batch>> GetPagedBatchesAsync(int pageIndex, int pageSize, BatchesSearch searchModel)
        {
            var query = _context.Batches.AsQueryable();

            // Фильтрация по ключевому слову
            if (!string.IsNullOrEmpty(searchModel.Keyword))
            {
                query = query.Where(b => b.Code.Contains(searchModel.Keyword) || b.Description.Contains(searchModel.Keyword));
            }

            // Фильтрация по статусу
            if (searchModel.Done.HasValue)
            {
                query = query.Where(b => b.Done == searchModel.Done);
            }

            var totalCount = await query.CountAsync();
            var items = await query.Skip((pageIndex - 1) * pageSize)
                                   .Take(pageSize)
                                   .ToListAsync();

            return new PagedResult<Batch>
            {
                Results = items,
                TotalCount = totalCount,
                PageSize = pageSize,
                PageIndex = pageIndex
            };
        }
    }
}
