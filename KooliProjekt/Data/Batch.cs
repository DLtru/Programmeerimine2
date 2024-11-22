using System.ComponentModel.DataAnnotations;

namespace KooliProjekt.Data
{
    public class Batch
    {
        public int Id { get; set; }

        public DateTime Date { get; set; }
        [Required]
        public string Code { get; set; }
        [Required]
        public string Description { get; set; }
        [Required]
        public Beer Beer { get; set; }
        [Required]
        public ICollection<LogEntry> LogEntri { get; set; }
        [Required]
        public ICollection<TastingEntry> TastingEntry { get; set; }
    }
}
