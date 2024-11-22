using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace KooliProjekt.Data
{
    public class TastingEntry
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public int Rating { get; set; }
        [Required]
        public string Comments { get; set; }

        public int BatchId { get; set; }
        public IdentityUser User { get; set; }
        public string UserId { get; set; }
        [Required]
        public Batch Batch { get; set; }
    }

}
