using KooliProjekt.Models;
using KooliProjekt.Search;

namespace KooliProjekt.Services
{
    public interface IPhotoService
    {
        Task<IEnumerable<Photo>> GetPagedPhotosAsync(int page, int pageSize);
        Task<Photo> GetPhotoByIdAsync(int id);
        Task CreatePhotoAsync(Photo photo);
        Task UpdatePhotoAsync(Photo photo);
        Task DeletePhotoAsync(int id);
        Task<IEnumerable<Photo>> GetPhotosBySearchAsync(PhotosSearch search, int page, int pageSize);
    }
}
