using KooliProjekt.Data;
using KooliProjekt.Models;
using KooliProjekt.Search;

namespace KooliProjekt.Services
{
    public interface IPhotoService
    {
        Task<Photo> GetPhotoByIdAsync(int id);
        Task CreatePhotoAsync(Photo photo);
        Task UpdatePhotoAsync(Photo photo);
        Task DeletePhotoAsync(int id);
        Task<PagedResult<Photo>> List(int page, int v, PhotosSearch searchModel);
    }
}