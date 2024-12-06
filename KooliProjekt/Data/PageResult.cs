
namespace KooliProjekt.Data
{
    public class PagedResult<T> : PagedResultBase where T : class
    {
        public IList<T> Results { get; set; }
        public int TotalCount { get; internal set; }
        public int PageIndex { get; internal set; }

        public PagedResult()
        {
            Results = new List<T>();
        }
    }
}