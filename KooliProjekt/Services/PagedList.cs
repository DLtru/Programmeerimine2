using System.Collections.Generic;

namespace KooliProjekt.Models
{
    public class PagedList<T>
    {
        public IEnumerable<T> Results { get; set; }
        public int Total { get; set; }
    }
}