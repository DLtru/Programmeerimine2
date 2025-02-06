using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using KooliProjekt.Models;
using KooliProjekt.Services;
using KooliProjekt.Search;

namespace KooliProjekt.Controllers
{
    public class LogEntriesController : Controller
    {
        private readonly ILogEntryService _logEntryService;

        public LogEntriesController(ILogEntryService logEntryService)
        {
            _logEntryService = logEntryService;
        }

        // GET: LogEntries
        public async Task<IActionResult> Index(LogEntriesSearch search, int page = 1, int pageSize = 5)
        {
            var logEntries = await _logEntryService.List(page, pageSize, search);

            var model = new LogEntriesIndexModel
            {
                Search = search,
                Data = logEntries
            };

            return View(model);
        }
    }
}
