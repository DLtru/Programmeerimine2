namespace KooliProjekt.Search
{
    public class BeersSearch
    {
        public string Code { get; set; }
        public string Name { get; set; }
        public double? MinAlcoholContent { get; set; }
        public double? MaxAlcoholContent { get; set; }
        public DateTime? FromBrewingDate { get; set; }
        public DateTime? ToBrewingDate { get; set; }
    }
}
