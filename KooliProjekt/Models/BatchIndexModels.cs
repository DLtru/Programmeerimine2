using KooliProjekt.Data;
using KooliProjekt.Search;

namespace KooliProjekt.Models
{
    public class BatchesIndexModel
    {
        public BatchesSearch Search { get; set; }
        public PagedResult<Batch> Data { get; set; }
    }
}
