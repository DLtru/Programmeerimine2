using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using KooliProjekt.Models;
using KooliProjekt.Services;
using KooliProjekt.Search;
using KooliProjekt.Data;

namespace KooliProjekt.Controllers
{
    public class BatchesController : Controller
    {
        private readonly IBatchService _batchService;

        public BatchesController(IBatchService batchService)
        {
            _batchService = batchService ?? throw new ArgumentNullException(nameof(batchService));
        }

        // GET: Batches
        public async Task<IActionResult> Index(int page = 1, string keyword = null, bool? done = null)
        {
            // Создаем модель поиска с фильтрами
            var searchModel = new BatchesSearch
            {
                Keyword = keyword,
                Done = done
            };

            // Получаем результаты с учетом пагинации и фильтров
            var pagedBatchesEntries = await _batchService.List(page, 5, searchModel);

            // Создаем модель для отображения с данными и поисковыми параметрами
            var model = new BatchesIndexModel
            {
                Search = searchModel,
                Data = pagedBatchesEntries
            };

            // Возвращаем представление с моделью
            return View(model);
        }

        // GET: Batches/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var batch = await _batchService.GetById(id.Value);
            if (batch == null)
            {
                return NotFound();
            }

            return View(batch);
        }

        // GET: Batches/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Batches/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Date,Code,Description")] Batch batch)
        {
            if (ModelState.IsValid)
            {
                await _batchService.AddBatchAsync(batch);
                return RedirectToAction(nameof(Index));
            }
            return View(batch);
        }

        // GET: Batches/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var batch = await _batchService.GetBatchByIdAsync(id.Value);
            if (batch == null)
            {
                return NotFound();
            }
            return View(batch);
        }

        // POST: Batches/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Date,Code,Description")] Batch batch)
        {
            if (id != batch.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    await _batchService.UpdateBatchAsync(batch);
                }
                catch (Exception)
                {
                    if (!BatchExists(batch.Id))
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
            return View(batch);
        }

        // GET: Batches/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var batch = await _batchService.GetById(id.Value);
            if (batch == null)
            {
                return NotFound();
            }

            return View(batch);
        }

        // POST: Batches/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _batchService.Delete(id);
            return RedirectToAction(nameof(Index));
        }

        private bool BatchExists(int id)
        {
            return _batchService.BatchExists(id);
        }

        public async Task<RedirectToActionResult> Edit(Batch updatedBatch)
        {
            throw new NotImplementedException();
        }
    }
}
