using System.Collections.Generic;
using KooliProjekt.Data;
using KooliProjekt.Search;
using KooliProjekt.Services;

namespace KooliProjekt.Models
{
    public class TastingEntriesIndexModel
    {
        public TastingEntriesSearch Search { get; set; }
        public PagedResult<TastingEntry> Data { get; set; }
    }
}
