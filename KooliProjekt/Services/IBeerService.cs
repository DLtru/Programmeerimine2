using KooliProjekt.Data;
using KooliProjekt.Search;

namespace KooliProjekt.Services
{
    public interface IBeerService
    {
        Task<PagedResult<Beer>> List(int page, int pageSize, BeerSearch searchModel = null);
        Task<Beer> GetBeerByIdAsync(int id);
        Task AddBeerAsync(Beer beer);
        Task UpdateBeerAsync(Beer beer);
        Task DeleteBeerAsync(int id);
    }
}