using Microsoft.AspNetCore.Identity;

namespace KooliProjekt.Data
{
    public class TastingEntry
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public int Rating { get; set; }
        public required string Comments { get; set; }

        public int BatchId { get; set; }
        public IdentityUser User { get; set; }
        public string UserId { get; set; }
        public required Batch Batch { get; set; } 
    }

}
