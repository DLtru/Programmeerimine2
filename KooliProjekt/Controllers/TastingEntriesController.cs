using KooliProjekt.Data;
using KooliProjekt.Models;
using KooliProjekt.Search;
using KooliProjekt.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Threading.Tasks;

namespace KooliProjekt.Controllers
{
    public class TastingEntriesController : Controller
    {
        private readonly ITastingEntryService _tastingEntryService;

        public TastingEntriesController(ITastingEntryService tastingEntryService)
        {
            _tastingEntryService = tastingEntryService;
        }

        // GET: TastingEntries/Create
        public async Task<IActionResult> Create()
        {
            var batches = await _tastingEntryService.GetBatches();
            var users = await _tastingEntryService.GetUsers();

            ViewData["BatchId"] = new SelectList(batches, "Id", "Code");
            ViewData["UserId"] = new SelectList(users, "Id", "Name");
            return View();
        }

        // POST: TastingEntries/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,BatchId,UserId,Rating,Comments,Date")] TastingEntry tastingEntry)
        {
            if (ModelState.IsValid)
            {
                await _tastingEntryService.AddTastingEntryAsync(tastingEntry);
                return RedirectToAction(nameof(Index));
            }

            var batches = await _tastingEntryService.GetBatches();
            var users = await _tastingEntryService.GetUsers();

            ViewData["BatchId"] = new SelectList(batches, "Id", "Code", tastingEntry.BatchId);
            ViewData["UserId"] = new SelectList(users, "Id", "Name", tastingEntry.UserId);
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

            var batches = await _tastingEntryService.GetBatches();
            var users = await _tastingEntryService.GetUsers();

            ViewData["BatchId"] = new SelectList(batches, "Id", "Code", tastingEntry.BatchId);
            ViewData["UserId"] = new SelectList(users, "Id", "Name", tastingEntry.UserId);
            return View(tastingEntry);
        }

        // POST: TastingEntries/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,BatchId,UserId,Rating,Comments,Date")] TastingEntry tastingEntry)
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

            var batches = await _tastingEntryService.GetBatches();
            var users = await _tastingEntryService.GetUsers();

            ViewData["BatchId"] = new SelectList(batches, "Id", "Code", tastingEntry.BatchId);
            ViewData["UserId"] = new SelectList(users, "Id", "Name", tastingEntry.UserId);
            return View(tastingEntry);
        }
    }
}