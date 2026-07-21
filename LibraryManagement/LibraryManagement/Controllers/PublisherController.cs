using LibraryManagement.Models;
using LibraryManagement.Repository.IRepository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LibraryManagement.Controllers
{
    [Authorize(Policy = "Publishers.View")]
    public class PublisherController : Controller
    {
        private readonly IUnitofWork _unitofWork;

        public PublisherController(IUnitofWork unitofWork)
        {
            _unitofWork = unitofWork;
        }

        public IActionResult Index()
        {
            List<Publisher> obj = _unitofWork.Publisher.GetAll().ToList();
            return View(obj);
        }

        [Authorize(Policy = "Publishers.Create")]
        public IActionResult Upsert(int? id)
        {
            if (id == null || id == 0)
            {
                return View();
            }
            else
            {
                var pub = _unitofWork.Publisher.Get(c => c.Id == id);
                if (pub == null) { return NotFound(); }
                return View(pub);
            }
        }

        [Authorize(Policy = "Publishers.Edit")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Upsert(Publisher pub)
        {
            if (!ModelState.IsValid)
            {
                return View(pub);
            }
            else
            {
                if (pub.Id == 0)
                {
                    _unitofWork.Publisher.Add(pub);
                    _unitofWork.Save();
                }
                else
                {
                    _unitofWork.Publisher.Update(pub);
                    _unitofWork.Save();
                }
                return RedirectToAction("Index");
            }
        }

        [Authorize(Policy = "Publishers.Delete")]
        public IActionResult Delete(int id)
        {
            return View(_unitofWork.Publisher.Get(c => c.Id == id));
        }

        [Authorize(Policy = "Publishers.Delete")]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeletePost(int id)
        {
            var pub = _unitofWork.Publisher.Get(c => c.Id == id);
            if (pub == null) { return NotFound(); }
            _unitofWork.Publisher.Remove(pub);
            _unitofWork.Save();
            return RedirectToAction("Index");
        }
    }
}
