using KooliProjekt.Data;
using KooliProjekt.Search;

namespace KooliProjekt.Models
{
    public class BeersIndexModels
    {
        public PagedResult<Beer> Beers { get; set; }
        public BeersSearch Search { get; set; }
    }
}
