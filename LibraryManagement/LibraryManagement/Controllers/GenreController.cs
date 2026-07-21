using LibraryManagement.Models;
using LibraryManagement.Repository.IRepository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LibraryManagement.Controllers
{
    [Authorize(Policy = "Genres.View")]
    public class GenreController : Controller
    {
        private readonly IUnitofWork _unitofWork;

        public GenreController(IUnitofWork unitofWork)
        {
            _unitofWork = unitofWork;
        }

        public IActionResult Index()
        {
            List<Genre> obj = _unitofWork.Genre.GetAll().ToList();
            return View(obj);
        }

        [Authorize(Policy = "Genres.Create")]
        public IActionResult Upsert(int? id)
        {
            if (id == null || id == 0)
            {
                return View();
            }
            else
            {
                var genre = _unitofWork.Genre.Get(c => c.Id == id);
                if (genre == null) { return NotFound(); }
                return View(genre);
            }
        }

        [Authorize(Policy = "Genres.Edit")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Upsert(Genre genre)
        {
            if (!ModelState.IsValid)
            {
                return View(genre);
            }
            else
            {
                if (genre.Id == 0)
                {
                    _unitofWork.Genre.Add(genre);
                    _unitofWork.Save();
                }
                else
                {
                    _unitofWork.Genre.Update(genre);
                    _unitofWork.Save();
                }
                return RedirectToAction("Index");
            }
        }

        [Authorize(Policy = "Genres.Delete")]
        public IActionResult Delete(int id)
        {
            return View(_unitofWork.Genre.Get(c => c.Id == id));
        }

        [Authorize(Policy = "Genres.Delete")]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeletePost(int id)
        {
            var genre = _unitofWork.Genre.Get(c => c.Id == id);
            if (genre == null) { return NotFound(); }
            _unitofWork.Genre.Remove(genre);
            _unitofWork.Save();
            return RedirectToAction("Index");
        }
    }
}
