﻿using System.ComponentModel.DataAnnotations;

namespace KooliProjekt.Models
{
    public class Beer
    {
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        public string Description { get; set; }

        [Required]
        public string Type { get; set; }
    }
}
