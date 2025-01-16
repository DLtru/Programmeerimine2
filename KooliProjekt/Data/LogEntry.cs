using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace KooliProjekt.Data
{
    public class LogEntry
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }
        [Required]
        public IdentityUser User { get; set; }
        [Required]
        public string UserId { get; set; }
        public IList<Ingredient> Ingredient { get; set; }

        public LogEntry()
        {
            Ingredient = new List<Ingredient>();
        }
        [Required]
        public Batch Batch { get; set; }
        public string Description { get; internal set; }
    }
}
