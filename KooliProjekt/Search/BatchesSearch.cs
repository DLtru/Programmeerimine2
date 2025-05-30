using System;

namespace KooliProjekt.Search
{
    public class BatchesSearch // Переименовано с BatchSearch на BatchesSearch
    {
        public string Keyword { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public string Code { get; set; }
        public bool? Done { get; internal set; }
    }
}