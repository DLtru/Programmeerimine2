using System.ComponentModel.DataAnnotations;

namespace KooliProjekt.Data
{
    public class Ingredient
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string Unit { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal Quantity { get; set; }
        public decimal AmountUsed { get; set; }

        [Required]
        public LogEntry LogEntry { get; set; }
    }
}
