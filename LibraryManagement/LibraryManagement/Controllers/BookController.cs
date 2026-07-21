using LibraryManagement.Models;
using LibraryManagement.Repository;
using LibraryManagement.Repository.IRepository;
using LibraryManagement.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata;

namespace LibraryManagement.Controllers
{
    [Authorize]
    public class BookController : Controller
    {
        private readonly IUnitofWork _unitofWork;
        private BookVM PopulateBookVM(BookVM vm)
        {
            vm.AuthorList = _unitofWork.Author.GetAll();
            vm.PublisherList = _unitofWork.Publisher.GetAll();
            vm.GenreList= _unitofWork.Genre.GetAll();

            return vm;
        }
        public BookController(IUnitofWork unitofWork)
        {
            _unitofWork = unitofWork;
        }

        [Authorize(Policy = "Books.View")]
        public IActionResult Index()
        {
            var VM = new BookVM
            {
                Books = _unitofWork.Book.GetAll(includeProperties : "Author,Publisher,BookGenres.Genre,BookIssues.Member").ToList()
            };
            return View(VM);
        }

        [Authorize(Policy = "Books.Create")]
        public IActionResult Upsert (int? id)
        {
            BookVM vm = new();
            if (id == null||id==0)
            {
                vm.Book = new Book();
            }
            else
            {
                vm.Book = _unitofWork.Book.Get(b => b.Id == id, includeProperties: "BookGenres");
                if (vm.Book == null)
                {
                    return NotFound();
                }
                vm.SelectedGenreIds=vm.Book.BookGenres.Select(g => g.GenreId).ToList();
            }
            PopulateBookVM(vm);
            return View(vm);
        }

        [Authorize(Policy = "Books.Edit")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Upsert(BookVM vm)
        {
            if (!ModelState.IsValid)
            {
                PopulateBookVM(vm);
                return View(vm);
            }


            if (vm.Book.Id == 0)
            {
                vm.Book.AvailableCopies = vm.Book.TotalCopies;

                foreach (var genreId in vm.SelectedGenreIds)
                {
                    vm.Book.BookGenres.Add(new BookGenre
                    {
                        GenreId = genreId
                    });
                }

                _unitofWork.Book.Add(vm.Book);
            }
            else
            {
                var bookFromDb = _unitofWork.Book.Get(
                    b => b.Id == vm.Book.Id,
                    includeProperties: "BookGenres"
                );

                int previouslyIssued = bookFromDb.TotalCopies - bookFromDb.AvailableCopies;

                bookFromDb.Title = vm.Book.Title;
                bookFromDb.ISBN = vm.Book.ISBN;
                bookFromDb.Description = vm.Book.Description;
                bookFromDb.AuthorId = vm.Book.AuthorId;
                bookFromDb.PublisherId = vm.Book.PublisherId;
                bookFromDb.PublicationYear = vm.Book.PublicationYear;
                bookFromDb.Edition = vm.Book.Edition;
                bookFromDb.Language = vm.Book.Language;
                bookFromDb.TotalCopies = vm.Book.TotalCopies;
                bookFromDb.AvailableCopies = vm.Book.TotalCopies - previouslyIssued;

                bookFromDb.BookGenres.Clear();

                foreach (var genreId in vm.SelectedGenreIds)
                {
                    bookFromDb.BookGenres.Add(new BookGenre
                    {
                        GenreId = genreId
                    });
                }

                _unitofWork.Book.Update(bookFromDb);
            }


            _unitofWork.Save();

            return RedirectToAction(nameof(Index));
        }

        [Authorize(Policy = "Books.Delete")]
        public IActionResult Delete(int id)
        {
            var book = _unitofWork.Book.Get(
                b => b.Id == id,
                includeProperties: "Author,Publisher"
            );

            if (book == null)
            {
                return NotFound();
            }

            return View(book);
        }

        [Authorize(Policy = "Books.Delete")]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeletePost(int id)
        {
            var book = _unitofWork.Book.Get(b => b.Id == id);

            if (book == null)
            {
                return NotFound();
            }


            _unitofWork.Book.DeleteBook(id);


            return RedirectToAction(nameof(Index));
        }
    }
}
