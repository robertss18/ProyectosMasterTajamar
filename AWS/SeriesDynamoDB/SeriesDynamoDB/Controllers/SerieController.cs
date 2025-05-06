using Microsoft.AspNetCore.Mvc;
using SeriesDynamoDB.Models;
using SeriesDynamoDB.Services;

namespace SeriesDynamoDB.Controllers
{
    public class SerieController : Controller
    {

        private readonly SerieService _service;

        public SerieController(SerieService service)
        {
            _service = service;
        }

        public async Task<IActionResult> Index()
        {
            var series = await _service.GetAllSeriesAsync();
            return View(series);
        }

        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(Serie serie)
        {
            if (ModelState.IsValid)
            {
                await _service.CreateSerieAsync(serie);
                return RedirectToAction("Index");
            }
            return View(serie);
        }

        public async Task<IActionResult> Edit(string id)
        {
            var serie = await _service.GetSerieByIdAsync(id);
            if (serie == null)
            {
                return NotFound();
            }
            return View(serie);
        }
        [HttpPost]
        public async Task<IActionResult> Edit(Serie serie)
        {
            if (ModelState.IsValid)
            {
                await _service.UpdateSerieAsync(serie);
                return RedirectToAction("Index");
            }
            return View(serie);
        }

        public async Task<IActionResult> Delete(string id)
        {
            var serie = await _service.GetSerieByIdAsync(id);
            if (serie == null)
            {
                return NotFound();
            }

            return View(serie);          

        }

        [HttpPost]
        public async Task<IActionResult> DeleteConfirmed(Serie serie)
        {
            await _service.DeleteSerieAsync(serie.SerieId!);
            return RedirectToAction("Index");
        }

    }
}
