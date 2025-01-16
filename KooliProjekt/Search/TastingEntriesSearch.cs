namespace KooliProjekt.Search
{
    public class TastingEntriesSearch
    {
        public string Keyword { get; set; }
        public int? MinRating { get; set; }
        public int? MaxRating { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
    }
}
