using KooliProjekt.Data;

namespace KooliProjekt.Services
{
    internal class PaginatedList<T>
    {
        public List<TastingEntry> results;
        private int totalCount;
        private int page;
        private int pageSize;

        public PaginatedList(List<TastingEntry> results, int totalCount, int page, int pageSize)
        {
            this.results = results;
            this.totalCount = totalCount;
            this.page = page;
            this.pageSize = pageSize;
        }
    }
}