
namespace KooliProjekt.Models
{
    public class PhotosIndexModel
    {
        public int Id { get; set; }
        public string Url { get; set; }
        public string Description { get; set; }
        public string FilePath { get; set; }
        public DateTime Date { get; internal set; }
    }
}
