using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using KooliProjekt.Data;
using KooliProjekt.Models;
using KooliProjekt.Search;
using KooliProjekt.Services;

namespace KooliProjekt.Controllers
{
    public class LogEntriesController : Controller
    {
        private readonly LogEntryService _logEntryService;

        public LogEntriesController(LogEntryService logEntryService)
        {
            _logEntryService = logEntryService;
        }

        // GET: LogEntries
        public async Task<IActionResult> Index(LogEntriesSearch search, int page = 1, int pageSize = 5)
        {
            var logEntries = await _logEntryService.GetPagedLogEntriesAsync(search, page, pageSize);

            var model = new LogEntriesIndexModel
            {
                Search = search,
                Data = logEntries
            };

            return View(model);
        }

        // GET: LogEntries/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var logEntry = await _logEntryService.GetLogEntryByIdAsync(id.Value);
            if (logEntry == null)
            {
                return NotFound();
            }

            return View(logEntry);
        }

        // GET: LogEntries/Create
        public IActionResult Create()
        {
            ViewData["UserId"] = new SelectList(_logEntryService.GetAllUsers(), "Id", "Name");
            return View();
        }

        // POST: LogEntries/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Date,Description,UserId")] LogEntry logEntry)
        {
            if (ModelState.IsValid)
            {
                await _logEntryService.CreateLogEntryAsync(logEntry);
                return RedirectToAction(nameof(Index));
            }
            ViewData["UserId"] = new SelectList(_logEntryService.GetAllUsers(), "Id", "Name", logEntry.UserId);
            return View(logEntry);
        }

        // GET: LogEntries/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var logEntry = await _logEntryService.GetLogEntryByIdAsync(id.Value);
            if (logEntry == null)
            {
                return NotFound();
            }

            ViewData["UserId"] = new SelectList(_logEntryService.GetAllUsers(), "Id", "Name", logEntry.UserId);
            return View(logEntry);
        }

        // POST: LogEntries/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Date,Description,UserId")] LogEntry logEntry)
        {
            if (id != logEntry.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    await _logEntryService.UpdateLogEntryAsync(logEntry);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!await LogEntryExists(logEntry.Id))
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
            ViewData["UserId"] = new SelectList(_logEntryService.GetAllUsers(), "Id", "Name", logEntry.UserId);
            return View(logEntry);
        }

        // GET: LogEntries/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var logEntry = await _logEntryService.GetLogEntryByIdAsync(id.Value);
            if (logEntry == null)
            {
                return NotFound();
            }

            return View(logEntry);
        }

        // POST: LogEntries/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _logEntryService.DeleteLogEntryAsync(id);
            return RedirectToAction(nameof(Index));
        }

        private async Task<bool> LogEntryExists(int id)
        {
            return await _logEntryService.LogEntryExistsAsync(id);
        }
    }
}