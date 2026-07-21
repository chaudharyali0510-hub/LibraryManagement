using LibraryManagement.Models;
using LibraryManagement.Repository.IRepository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LibraryManagement.Controllers
{
    [Authorize]
    public class AuthorController : Controller
    {
        private readonly IUnitofWork _unitofWork;

        public AuthorController(IUnitofWork unitofWork)
        {
            _unitofWork = unitofWork;
        }

        public IActionResult Index()
        {
            List<Author> obj = _unitofWork.Author.GetAll().ToList();
            return View(obj);
        }
        public IActionResult Upsert(int? id)
        {
            if (id == null || id == 0)
            {
                return View();
            }
            else
            {
                var author = _unitofWork.Author.Get(c => c.Id == id);
                if (author == null) { return NotFound(); }
                return View(author);
            }
        }
        [HttpPost]
        public IActionResult Upsert(Author author)
        {
            if (!ModelState.IsValid)
            {
                return View(author);
            }
            else
            {
                if (author.Id == 0)
                {
                    _unitofWork.Author.Add(author);
                    _unitofWork.Save();
                }
                else
                {
                    _unitofWork.Author.Update(author);
                    _unitofWork.Save();
                }
                return RedirectToAction("Index");
            }
        }
        public IActionResult Delete(int id)
        {
            return View(_unitofWork.Author.Get(c => c.Id == id));
        }

        [HttpPost, ActionName("Delete")]
        public IActionResult DeletePost(int id)
        {
            var author = _unitofWork.Author.Get(c => c.Id == id);
            if (author == null) { return NotFound(); }
            _unitofWork.Author.Remove(author);
            _unitofWork.Save();
            return RedirectToAction("Index");
        }
    }
}

