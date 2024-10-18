namespace KooliProjekt.Data
{
    public class Batch
    {
        public int Id { get; set; }

        public DateTime Date { get; set; }
        public required string Code { get; set; }
        public required string Description { get; set; }
        public required Beer Beer { get; set; }
        public required ICollection<LogEntry> LogEntri { get; set; }
        public required ICollection<TastingEntry> TastingEntry { get; set; }
    }
}
