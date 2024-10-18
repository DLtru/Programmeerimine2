using Microsoft.AspNetCore.Identity;

namespace KooliProjekt.Data
{
    public class LogEntry
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public required string Desctipyion { get; set; }
        public required IdentityUser User { get; set; }
        public required string UserId { get; set; }
        public IList<Ingredient> Ingredient { get; set; }

        public LogEntry()
        {
            Ingredient = new List<Ingredient>();
        }
        public required Batch Batch { get; set; }
    }
}
