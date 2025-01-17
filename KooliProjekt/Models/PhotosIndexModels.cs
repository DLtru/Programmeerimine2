﻿
namespace KooliProjekt.Models
{
    public class Photo
    {
        public int Id { get; set; }
        public string Url { get; set; }
        public string Description { get; set; }
        public string FilePath { get; set; }
        public DateTime Date { get; internal set; }
    }
}
