using Microsoft.AspNetCore.Mvc;
using KooliProjekt.Services;
using KooliProjekt.Data;

namespace KooliProjekt.Controllers
{
    public class BeersController : Controller
    {
        private readonly IBeerService _beerService;

        public BeersController(IBeerService beerService)
        {
            _beerService = beerService;
        }

        // GET: Beers
        public async Task<IActionResult> Index(int page = 1, string searchName = "", string searchType = "")
        {
            var searchModel = new BeerSearch
            {
                Name = searchName,
                Type = searchType
            };

            var pagedBeers = await _beerService.GetPagedBeersAsync(page, 5, searchModel);
            ViewData["SearchName"] = searchName;
            ViewData["SearchType"] = searchType;

            return View(pagedBeers);
        }

        // GET: Beers/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var beer = await _beerService.GetBeerByIdAsync(id.Value);
            if (beer == null)
            {
                return NotFound();
            }

            return View(beer);
        }

        // GET: Beers/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Beers/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Description,Type")] Beer beer)
        {
            if (ModelState.IsValid)
            {
                await _beerService.AddBeerAsync(beer);
                return RedirectToAction(nameof(Index));
            }
            return View(beer);
        }

        // GET: Beers/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var beer = await _beerService.GetBeerByIdAsync(id.Value);
            if (beer == null)
            {
                return NotFound();
            }
            return View(beer);
        }

        // POST: Beers/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Description,Type")] Beer beer)
        {
            if (id != beer.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                await _beerService.UpdateBeerAsync(beer);
                return RedirectToAction(nameof(Index));
            }
            return View(beer);
        }

        // GET: Beers/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var beer = await _beerService.GetBeerByIdAsync(id.Value);
            if (beer == null)
            {
                return NotFound();
            }

            return View(beer);
        }

        // POST: Beers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _beerService.DeleteBeerAsync(id);
            return RedirectToAction(nameof(Index));
        }
    }
}
