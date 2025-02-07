using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace KooliProjekt.Data
{
    public static class PagingExtensions
    {
        public static async Task<PagedResult<T>> GetPagedAsync<T>(this IQueryable<T> query, int page, int pageSize) where T : class
        {
            page = Math.Max(page, 1);
            pageSize = pageSize == 0 ? 10 : pageSize;

            var rowCount = await query.CountAsync();
            var pageCount = (int)Math.Ceiling((double)rowCount / pageSize);

            var results = await query.Skip((page - 1) * pageSize).Take(pageSize).ToListAsync();

            return new PagedResult<T>(results, rowCount, page, pageSize) { PageCount = pageCount };
        }
    }
}
