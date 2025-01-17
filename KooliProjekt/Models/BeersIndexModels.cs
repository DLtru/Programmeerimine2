using KooliProjekt.Controllers;
using KooliProjekt.Data;

namespace KooliProjekt.Models
{
    public class BeersIndexModels
    {
        public PagedResult<Beer> Beers { get; set; }
        public BeerSearch Search { get; set; }
    }
}
