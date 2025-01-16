

namespace KooliProjekt.Data
{
    public class PagedResult<T> : PagedResultBase where T : class
    {
        public IList<T> Results { get; set; }
        public int TotalCount { get; internal set; }
        public int PageIndex { get; internal set; }
        public int Page { get; internal set; }
        public int TotalItems { get; internal set; }
        public List<Batch> Items { get; internal set; }

        public PagedResult()
        {
            Results = new List<T>();
        }

        public static implicit operator PagedResult<T>(Services.PagedResult<Batch> v)
        {
            throw new NotImplementedException();
        }
    }
}