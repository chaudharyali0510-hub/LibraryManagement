using LibraryManagement.Models;
using LibraryManagement.Repository.IRepository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace LibraryManagement.Controllers
{
    [Authorize]
    public class BookIssueController : Controller
    {
        private readonly IUnitofWork _unitOfWork;

        public BookIssueController(IUnitofWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public IActionResult Index()
        {
            var issues = _unitOfWork.BookIssue
                .GetAll(includeProperties: "Book,Member")
                .ToList();

            return View(issues);
        }

        private void PopulateDropdowns()
        {
            ViewBag.Books = _unitOfWork.Book
                .GetAll()
                .Where(b => b.AvailableCopies > 0)
                .Select(b => new SelectListItem
                {
                    Text = b.Title,
                    Value = b.Id.ToString()
                });


            ViewBag.Members = _unitOfWork.Member
                .GetAll()
                .Select(m => new SelectListItem
                {
                    Text = $"{m.FirstName} {m.LastName}",
                    Value = m.Id.ToString()
                });
        }

        public IActionResult Upsert(int? id, int? bookId)
        {
            PopulateDropdowns();


            if (bookId != null)
            {
                return View(new BookIssue
                {
                    BookId = bookId.Value,
                    DateIssue = DateTime.Now,
                    DueDate = DateTime.Now.AddDays(7)
                });
            }


            if (id == null || id == 0)
            {
                return View(new BookIssue
                {
                    DateIssue = DateTime.Now,
                    DueDate = DateTime.Now.AddDays(7)
                });
            }


            var issue = _unitOfWork.BookIssue.Get(
                x => x.Id == id,
                includeProperties: "Book,Member"
            );


            if (issue == null)
            {
                return NotFound();
            }

            PopulateDropdowns();

            return View(issue);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Upsert(BookIssue issue)
        {
            if (!ModelState.IsValid)
            {
                PopulateDropdowns();
                return View(issue);
            }



            if (issue.Id == 0)
            {
                var book = _unitOfWork.Book.Get(
                    b => b.Id == issue.BookId
                );


                if (book == null)
                {
                    return NotFound();
                }


                if (book.AvailableCopies <= 0)
                {
                    ModelState.AddModelError(
                        "",
                        "Book is not available"
                    );

                    PopulateDropdowns();

                    return View(issue);
                }



                book.AvailableCopies--;


                _unitOfWork.Book.Update(book);


                issue.isReturned = false;


                _unitOfWork.BookIssue.Add(issue);
            }
            else
            {
                _unitOfWork.BookIssue.Update(issue);
            }



            _unitOfWork.Save();


            return RedirectToAction(nameof(Index));
        }





        // Return Book
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult ReturnBook(int id)
        {
            var issue = _unitOfWork.BookIssue.Get(
                x => x.Id == id,
                includeProperties: "Book"
            );


            if (issue == null)
            {
                return NotFound();
            }



            if (issue.isReturned)
            {
                return BadRequest(
                    "Book already returned"
                );
            }



            issue.ReturnDate = DateTime.Now;

            issue.isReturned = true;



            if (issue.ReturnDate > issue.DueDate)
            {
                int daysLate =
                    (issue.ReturnDate.Value - issue.DueDate).Days;


                issue.FineAmount = daysLate * 2;
            }



            issue.Book.AvailableCopies++;


            _unitOfWork.Book.Update(issue.Book);

            _unitOfWork.BookIssue.Update(issue);


            _unitOfWork.Save();



            return RedirectToAction(nameof(Index));
        }





        // Delete confirmation page
        public IActionResult Delete(int id)
        {
            var issue = _unitOfWork.BookIssue.Get(
                x => x.Id == id,
                includeProperties: "Book,Member"
            );


            if (issue == null)
            {
                return NotFound();
            }


            return View(issue);
        }




        // Delete Issue
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeletePost(int id)
        {
            var issue = _unitOfWork.BookIssue.Get(
                x => x.Id == id
            );


            if (issue == null)
            {
                return NotFound();
            }



            // Restore book copies if not returned
            if (!issue.isReturned)
            {
                var book = _unitOfWork.Book.Get(
                    b => b.Id == issue.BookId
                );


                if (book != null)
                {
                    book.AvailableCopies++;

                    _unitOfWork.Book.Update(book);
                }
            }



            _unitOfWork.BookIssue.Remove(issue);


            _unitOfWork.Save();



            return RedirectToAction(nameof(Index));
        }
    }
}