using System.Collections.Generic;

namespace KooliProjekt.Data
{
    public class PagedResult<T> : PagedResultBase where T : class
    {
        public IList<T> Results { get; set; }
        public int TotalCount { get; set; }
        public int PageIndex { get; set; }
        public int Page { get; set; }
        public int TotalItems { get; set; }
        public int PageNumber { get; set; }
       
    }
}
