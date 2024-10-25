using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using KooliProjekt.Data;

namespace KooliProjekt.Controllers
{
    public class TastingEntriesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public TastingEntriesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: TastingEntries
        public async Task<IActionResult> Index(int page = 1)
        {
            var pagedTastingEntries = await _context.TastingEntries.GetPagedAsync (page, 5);
            return View(pagedTastingEntries);
        }

        // GET: TastingEntries/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tastingEntry = await _context.TastingEntries
                .Include(t => t.Batch)
                .Include(t => t.User)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (tastingEntry == null)
            {
                return NotFound();
            }

            return View(tastingEntry);
        }

        // GET: TastingEntries/Create
        public IActionResult Create()
        {
            ViewData["BatchId"] = new SelectList(_context.Batches, "Id", "Id");
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Id");
            return View();
        }

        // POST: TastingEntries/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Date,Rating,Comments,BatchId,UserId")] TastingEntry tastingEntry)
        {
            if (ModelState.IsValid)
            {
                _context.Add(tastingEntry);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["BatchId"] = new SelectList(_context.Batches, "Id", "Id", tastingEntry.BatchId);
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Id", tastingEntry.UserId);
            return View(tastingEntry);
        }

        // GET: TastingEntries/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tastingEntry = await _context.TastingEntries.FindAsync(id);
            if (tastingEntry == null)
            {
                return NotFound();
            }
            ViewData["BatchId"] = new SelectList(_context.Batches, "Id", "Id", tastingEntry.BatchId);
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Id", tastingEntry.UserId);
            return View(tastingEntry);
        }

        // POST: TastingEntries/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
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
                try
                {
                    _context.Update(tastingEntry);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TastingEntryExists(tastingEntry.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["BatchId"] = new SelectList(_context.Batches, "Id", "Id", tastingEntry.BatchId);
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Id", tastingEntry.UserId);
            return View(tastingEntry);
        }

        // GET: TastingEntries/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tastingEntry = await _context.TastingEntries
                .Include(t => t.Batch)
                .Include(t => t.User)
                .FirstOrDefaultAsync(m => m.Id == id);
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
            var tastingEntry = await _context.TastingEntries.FindAsync(id);
            if (tastingEntry != null)
            {
                _context.TastingEntries.Remove(tastingEntry);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TastingEntryExists(int id)
        {
            return _context.TastingEntries.Any(e => e.Id == id);
        }
    }
}
