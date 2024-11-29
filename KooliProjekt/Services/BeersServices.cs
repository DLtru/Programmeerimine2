using KooliProjekt.Data;
using Microsoft.EntityFrameworkCore;
using KooliProjekt.Services;
using Microsoft.AspNetCore.Mvc;
using KooliProjekt.Models;
using System.Threading.Tasks;
namespace KooliProjekt.Services

{
    public interface IBeerService
    {
        Task<List<Beer>> GetPagedBeersAsync(int page, int pageSize);
        Task<Beer> GetBeerByIdAsync(int id);
        Task AddBeerAsync(Beer beer);
        Task UpdateBeerAsync(Beer beer);
        Task DeleteBeerAsync(int id);
    }
}

    public class BeerService : IBeerService
    {
        private readonly ApplicationDbContext _context;

        public BeerService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<Beer>> GetPagedBeersAsync(int page, int pageSize)
        {
            return await _context.Beers
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
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
            _context.Beers.Update(beer);
            await _context.SaveChangesAsync();
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

