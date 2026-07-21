using LibraryManagement.Models;
using LibraryManagement.Repository.IRepository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LibraryManagement.Controllers
{
    [Authorize(Policy = "Members.View")]
    public class MemberController : Controller
    {
        private readonly IUnitofWork _unitOfWork;

        public MemberController(IUnitofWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public IActionResult Index()
        {
            List<Member> members = _unitOfWork.Member.GetAll().ToList();
            return View(members);
        }

        [Authorize(Policy = "Members.Create")]
        public IActionResult Upsert(int? id)
        {
            if (id == null || id == 0)
            {
                return View(new Member());
            }

            var member = _unitOfWork.Member.Get(m => m.Id == id);

            if (member == null)
            {
                return NotFound();
            }

            return View(member);
        }

        [Authorize(Policy = "Members.Edit")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Upsert(Member member)
        {
            if (!ModelState.IsValid)
            {
                return View(member);
            }

            if (member.Id == 0)
            {
                _unitOfWork.Member.Add(member);
            }
            else
            {
                _unitOfWork.Member.Update(member);
            }

            _unitOfWork.Save();

            return RedirectToAction(nameof(Index));
        }

        [Authorize(Policy = "Members.Delete")]
        public IActionResult Delete(int id)
        {
            var member = _unitOfWork.Member.Get(m => m.Id == id);

            if (member == null)
            {
                return NotFound();
            }

            return View(member);
        }

        [Authorize(Policy = "Members.Delete")]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeletePost(int id)
        {
            var member = _unitOfWork.Member.Get(m => m.Id == id);

            if (member == null)
            {
                return NotFound();
            }

            _unitOfWork.Member.Remove(member);

            _unitOfWork.Save();

            return RedirectToAction(nameof(Index));
        }
    }
}
