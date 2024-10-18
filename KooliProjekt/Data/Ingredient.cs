namespace KooliProjekt.Data
{
    public class Ingredient
    {
        public int Id { get; set; }
        public required string Name { get; set; }
        public required string Unit { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal Quantity { get; set; }
        public decimal AmountUsed { get; set; }
        public required LogEntry LogEntry { get; set; }
    }
}
