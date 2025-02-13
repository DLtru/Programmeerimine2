using System.Collections.Generic;

namespace KooliProjekt.Data
{
    public class PagedResult<T> : PagedResultBase where T : class
    {
        private List<T> results;

        public PagedResult()
        {
        }

        public PagedResult(List<T> results, int rowCount, int page, int pageSize)
        {
            this.results = results;
            RowCount = rowCount;
            Page = page;
            PageSize = pageSize;
        }

        public IList<T> Results { get; set; }
        public int TotalCount { get; set; }
        public int PageIndex { get; set; }
        public int Page { get; set; }
        public int TotalItems { get; set; }
        public int PageNumber { get; set; }
       
    }
}
