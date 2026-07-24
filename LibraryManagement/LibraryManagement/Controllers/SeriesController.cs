using LibraryManagement.Models;
using LibraryManagement.Repository.IRepository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LibraryManagement.Controllers
{
    [Authorize(Policy = "Series.View")]
    public class SeriesController : Controller
    {
        private readonly IUnitofWork _unitofWork;

        public SeriesController(IUnitofWork unitofWork)
        {
            _unitofWork = unitofWork;
        }

        public IActionResult Index()
        {
            List<Series> obj = _unitofWork.Series.GetAll().ToList();
            return View(obj);
        }

        [Authorize(Policy = "Series.Create")]
        public IActionResult Upsert(int? id)
        {
            if (id == null || id == 0)
            {
                return View();
            }
            else
            {
                var series = _unitofWork.Series.Get(s => s.Id == id);
                if (series == null) { return NotFound(); }
                return View(series);
            }
        }

        [Authorize(Policy = "Series.Edit")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Upsert(Series series)
        {
            if (!ModelState.IsValid)
            {
                return View(series);
            }
            else
            {
                if (series.Id == 0)
                {
                    _unitofWork.Series.Add(series);
                    _unitofWork.Save();
                }
                else
                {
                    _unitofWork.Series.Update(series);
                    _unitofWork.Save();
                }
                return RedirectToAction("Index");
            }
        }

        [Authorize(Policy = "Series.Delete")]
        public IActionResult Delete(int id)
        {
            return View(_unitofWork.Series.Get(s => s.Id == id));
        }

        [Authorize(Policy = "Series.Delete")]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeletePost(int id)
        {
            var series = _unitofWork.Series.Get(s => s.Id == id);
            if (series == null) { return NotFound(); }
            _unitofWork.Series.Remove(series);
            _unitofWork.Save();
            return RedirectToAction("Index");
        }
    }
}
