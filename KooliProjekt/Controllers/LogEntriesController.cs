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

        public async Task<ViewResult> Index(int page)
        {
            throw new NotImplementedException();
        }
    }
}