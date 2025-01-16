using KooliProjekt.Data;
using KooliProjekt.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace KooliProjekt.Services
{
    public interface IBeerService
    {
        Task<PagedResult<Beer>> GetPagedBeersAsync(int page, int pageSize, BeerSearch searchModel = null);
        Task<Beer> GetBeerByIdAsync(int id);
        Task AddBeerAsync(Beer beer);
        Task UpdateBeerAsync(Beer beer);
        Task DeleteBeerAsync(int id);
    }

    public class BeerService : IBeerService
    {
        private readonly ApplicationDbContext _context;

        public BeerService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<PagedResult<Beer>> GetPagedBeersAsync(int page, int pageSize, BeerSearch searchModel = null)
        {
            var query = _context.Beers.AsQueryable();

            // Применение фильтра по имени или типу, если передан searchModel
            if (searchModel != null)
            {
                if (!string.IsNullOrEmpty(searchModel.Name))
                {
                    query = query.Where(b => b.Name.Contains(searchModel.Name));
                }

                if (!string.IsNullOrEmpty(searchModel.Type))
                {
                    query = query.Where(b => b.Type.Contains(searchModel.Type));
                }
            }

            // Получаем общее количество записей
            var totalCount = await query.CountAsync();

            // Применяем пагинацию
            var beers = await query
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return new PagedResult<Beer>
            {
                Results = beers,
                TotalCount = totalCount,
                PageIndex = page,
                PageSize = pageSize
            };
        }

        public async Task<Beer> GetBeerByIdAsync(int id)
        {
            return await _context.Beers
                .FirstOrDefaultAsync(b => b.Id == id);
        }

        public async Task AddBeerAsync(Beer beer)
        {
            _context.Beers.Add(beer);

            await _context.SaveChangesAsync();
        }




        public async Task UpdateBeerAsync(Beer beer)
        {
            var existingBeer = await _context.Beers.FindAsync(beer.Id);
            if (existingBeer != null)
            {
                existingBeer.Name = beer.Name;
                existingBeer.Description = beer.Description;
                existingBeer.Type = beer.Type;

                // Можно обновлять только те поля, которые были изменены
                _context.Beers.Update(existingBeer);
                await _context.SaveChangesAsync();
            }
        }

        public async Task DeleteBeerAsync(int id)
        {
            var beer = await _context.Beers.FindAsync(id);
            if (beer != null)
            {
                _context.Beers.Remove(beer);
                await _context.SaveChangesAsync();
            }
        }
    }

    // Пагинированный результат
    public class PagedResult<T>
    {
        public List<T> Results { get; set; }
        public int TotalCount { get; set; }
        public int PageIndex { get; set; }
        public int PageSize { get; set; }
    }

    // Модель поиска для пив
    public class BeerSearch
    {
        public string Name { get; set; }
        public string Type { get; set; }
    }
}
