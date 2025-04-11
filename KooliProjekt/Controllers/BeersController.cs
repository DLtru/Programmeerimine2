using Microsoft.AspNetCore.Mvc;
using KooliProjekt.Services;
using KooliProjekt.Data;
using KooliProjekt.Search;
using System;
using System.Threading.Tasks;

namespace KooliProjekt.Controllers
{
    public class BeersController : Controller
    {
        private readonly IBeerService _beerService;

        public BeersController(IBeerService beerService)
        {
            _beerService = beerService ?? throw new ArgumentNullException(nameof(beerService));
        }

        // GET: Beers
        public async Task<IActionResult> Index(int page = 1, string searchName = "", string searchType = "")
        {
            var searchModel = new BeersSearch
            {
                Name = searchName,
                //Type = searchType
            };

            // Получаем список пива с учетом пагинации и поиска
            var pagedBeers = await _beerService.List(page, 5, searchModel);

            // Логирование для отладки
            Console.WriteLine($"Index method called. Search: Name={searchName}, Type={searchType}, Page={page}");
            Console.WriteLine($"Retrieved {pagedBeers?.Results?.Count ?? 0} beers");

            // Если результат пуст, показываем ошибку
            if (pagedBeers == null)
            {
                return View("Error");
            }

            // Заполнение ViewData для поиска
            ViewData["SearchName"] = searchName;
            ViewData["SearchType"] = searchType;

            // Возвращаем представление с результатами
            return View(pagedBeers);
        }

        // Новый метод Index, исправленный
        public async Task<IActionResult> Index(int page, BeersSearch searchParams)
        {
            var pagedBeers = await _beerService.List(page, 5, searchParams);
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
                Console.WriteLine($"Beer created: {beer.Name}");
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
        public async Task<IActionResult> Edit(int id, Beer beer)
        {
            if (id != beer.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                await _beerService.UpdateBeerAsync(beer);
                Console.WriteLine($"Beer updated: {beer.Name}");
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
            await _beerService.Delete(id);
            Console.WriteLine($"Beer deleted: ID={id}");
            return RedirectToAction(nameof(Index));
        }
    }
}
