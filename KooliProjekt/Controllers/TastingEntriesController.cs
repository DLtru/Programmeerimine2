using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using KooliProjekt.Models;
using KooliProjekt.Services;
using KooliProjekt.Data;
using KooliProjekt.Search;

namespace KooliProjekt.Controllers
{
    public class TastingEntriesController : Controller
    {
        private readonly ITastingEntryService _tastingEntryService;

        public TastingEntriesController(ITastingEntryService tastingEntryService)
        {
            _tastingEntryService = tastingEntryService;
        }

        // GET: TastingEntries
        public async Task<IActionResult> Index(TastingEntriesSearch search, int page = 1)
        {
            var model = await _tastingEntryService.List(page, 5, search);
            return View(model);
        }

        // GET: TastingEntries/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tastingEntry = await _tastingEntryService.GetTastingEntryByIdAsync(id.Value);
            if (tastingEntry == null)
            {
                return NotFound();
            }

            return View(tastingEntry);
        }

        // GET: TastingEntries/Create
        public IActionResult Create()
        {
            ViewData["BatchId"] = new SelectList(_tastingEntryService.GetBatches(), "Id", "Id");
            ViewData["UserId"] = new SelectList(_tastingEntryService.GetUsers(), "Id", "Id");
            return View();
        }

        // POST: TastingEntries/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Date,Rating,Comments,BatchId,UserId")] TastingEntry tastingEntry)
        {
            if (ModelState.IsValid)
            {
                await _tastingEntryService.AddTastingEntryAsync(tastingEntry);
                return RedirectToAction(nameof(Index));
            }
            ViewData["BatchId"] = new SelectList(_tastingEntryService.GetBatches(), "Id", "Id", tastingEntry.BatchId);
            ViewData["UserId"] = new SelectList(_tastingEntryService.GetUsers(), "Id", "Id", tastingEntry.UserId);
            return View(tastingEntry);
        }

        // GET: TastingEntries/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tastingEntry = await _tastingEntryService.GetTastingEntryByIdAsync(id.Value);
            if (tastingEntry == null)
            {
                return NotFound();
            }
            ViewData["BatchId"] = new SelectList(_tastingEntryService.GetBatches(), "Id", "Id", tastingEntry.BatchId);
            ViewData["UserId"] = new SelectList(_tastingEntryService.GetUsers(), "Id", "Id", tastingEntry.UserId);
            return View(tastingEntry);
        }

        // POST: TastingEntries/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Date,Rating,Comments,BatchId,UserId")] TastingEntry tastingEntry)
        {
            if (id != tastingEntry.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                await _tastingEntryService.UpdateTastingEntryAsync(tastingEntry);
                return RedirectToAction(nameof(Index));
            }
            ViewData["BatchId"] = new SelectList(_tastingEntryService.GetBatches(), "Id", "Id", tastingEntry.BatchId);
            ViewData["UserId"] = new SelectList(_tastingEntryService.GetUsers(), "Id", "Id", tastingEntry.UserId);
            return View(tastingEntry);
        }

        // GET: TastingEntries/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tastingEntry = await _tastingEntryService.GetTastingEntryByIdAsync(id.Value);
            if (tastingEntry == null)
            {
                return NotFound();
            }

            return View(tastingEntry);
        }

        // POST: TastingEntries/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var tastingEntry = await _tastingEntryService.GetTastingEntryByIdAsync(id);
            if(tastingEntry == null)
            {
                return NotFound();
            }

            await _tastingEntryService.DeleteTastingEntryAsync(id);
            return RedirectToAction(nameof(Index));
        }
    }
}
