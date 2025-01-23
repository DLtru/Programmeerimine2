using KooliProjekt.Models;
using KooliProjekt.Services;
using KooliProjekt.Search;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using KooliProjekt.Data;

namespace KooliProjekt.Controllers
{
    public class PhotosController : Controller
    {
        private readonly IPhotoService _photoService;

        public PhotosController(IPhotoService photoService)
        {
            _photoService = photoService;
        }

        // Index page with paginated photos and search
        public async Task<IActionResult> Index(int page = 1, int pageSize = 10, PhotosSearch search = null)
        {
            var photos = await _photoService.GetPhotosBySearchAsync(search ?? new PhotosSearch(), page, pageSize);
            return View(photos);
        }

        // Create new photo
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Photo photo)
        {
            if (ModelState.IsValid)
            {
                await _photoService.CreatePhotoAsync(photo);
                return RedirectToAction(nameof(Index));
            }
            return View(photo);
        }

        // Edit existing photo
        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var photo = await _photoService.GetPhotoByIdAsync(id);
            if (photo == null)
            {
                return NotFound();
            }
            return View(photo);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Photo photo)
        {
            if (id != photo.Id)
            {
                return BadRequest();
            }

            if (ModelState.IsValid)
            {
                await _photoService.UpdatePhotoAsync(photo);
                return RedirectToAction(nameof(Index));
            }
            return View(photo);
        }

        // Delete photo
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _photoService.DeletePhotoAsync(id);
            return RedirectToAction(nameof(Index));
        }

        // View photo details
        public async Task<IActionResult> Details(int id)
        {
            var photo = await _photoService.GetPhotoByIdAsync(id);
            if (photo == null)
            {
                return NotFound();
            }
            return View(photo);
        }
    }
}
