﻿using KooliProjekt.Models;
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
        public bool Done { get; set; }
        public string Titles { get; set; }
        public string Title { get; set; }
    }
}
