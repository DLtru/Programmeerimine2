using KooliProjekt.Controllers;
using KooliProjekt.Data;
using KooliProjekt.Models;
using KooliProjekt.Search;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace KooliProjekt.Services
{
    public class BeerService : IBeerService
    {
        private readonly ApplicationDbContext _context;

        public BeerService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<PagedResult<Beer>> List(int page, int pageSize, BeerSearch searchModel = null)
        {
            var query = _context.Beers.AsQueryable();

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

            return await query.GetPagedAsync(page, pageSize);
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
}