using LibraryManagement.Models;
using LibraryManagement.Repository.IRepository;
using LibraryManagement.Utility;
using LibraryManagement.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using X.PagedList;
using X.PagedList.Mvc.Core;

namespace LibraryManagement.Controllers
{
    [Authorize]
    public class BookController : Controller
    {
        private readonly IUnitofWork _unitofWork;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public BookController(IUnitofWork unitofWork, IWebHostEnvironment webHostEnvironment)
        {
            _unitofWork = unitofWork;
            _webHostEnvironment = webHostEnvironment;
        }

        private BookVM PopulateBookVM(BookVM vm)
        {
            vm.AuthorList = _unitofWork.Author.GetAll();
            vm.PublisherList = _unitofWork.Publisher.GetAll();
            vm.GenreList = _unitofWork.Genre.GetAll();
            return vm;
        }

        private void PopulateFilterSelectLists(BookFilterVM filter)
        {
            var authors = _unitofWork.Author.GetAll().OrderBy(a => a.LastName);
            var publishers = _unitofWork.Publisher.GetAll().OrderBy(p => p.Name);
            var genres = _unitofWork.Genre.GetAll().OrderBy(g => g.Name);

            filter.Authors = new SelectList(authors, "Id", "FirstName", filter.AuthorId);
            filter.Publishers = new SelectList(publishers, "Id", "Name", filter.PublisherId);
            filter.Genres = new SelectList(genres, "Id", "Name", filter.GenreId);
        }

        [Authorize(Policy = "Books.View")]
        public async Task<IActionResult> Index(BookFilterVM filter)
        {
            filter.Page = Math.Max(1, filter.Page);

            var query = _unitofWork.Book.GetFilteredBooks(filter);

            var totalCount = await query.CountAsync();
            var items = await query
                .Skip((filter.Page - 1) * filter.PageSize)
                .Take(filter.PageSize)
                .ToListAsync();
            var pagedList = new StaticPagedList<Book>(items, filter.Page, filter.PageSize, totalCount);

            filter.Authors = new SelectList(
                _unitofWork.Author.GetAll().OrderBy(a => a.LastName), "Id", "FirstName", filter.AuthorId);
            filter.Publishers = new SelectList(
                _unitofWork.Publisher.GetAll().OrderBy(p => p.Name), "Id", "Name", filter.PublisherId);
            filter.Genres = new SelectList(
                _unitofWork.Genre.GetAll().OrderBy(g => g.Name), "Id", "Name", filter.GenreId);

            ViewBag.Filter = filter;

            return View(pagedList);
        }

        [Authorize(Policy = "Books.View")]
        public async Task<IActionResult> Details(int id)
        {
            var book = await _unitofWork.Book.GetBookDetailsAsync(id);
            if (book == null) return NotFound();

            var vm = new BookDetailsVM
            {
                Book = book,
                ActiveIssues = book.BookIssues.Where(bi => !bi.isReturned).OrderByDescending(bi => bi.DateIssue).ToList(),
                IssueHistory = book.BookIssues.Where(bi => bi.isReturned).OrderByDescending(bi => bi.ReturnDate).ToList()
            };

            return View(vm);
        }

        [Authorize(Policy = "Books.Create")]
        public IActionResult Upsert(int? id)
        {
            BookVM vm = new();
            if (id == null || id == 0)
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
                vm.SelectedGenreIds = vm.Book.BookGenres.Select(g => g.GenreId).ToList();
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
                if (vm.ImageFile != null)
                {
                    string imageUrl = FileUpload.UploadImage(
                        vm.ImageFile,
                        _webHostEnvironment.WebRootPath);

                    vm.Book.CoverImageUrl = imageUrl;
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
                if (vm.ImageFile != null)
                {
                    FileUpload.DeleteImage(
                        _webHostEnvironment.WebRootPath,
                        bookFromDb.CoverImageUrl);

                    bookFromDb.CoverImageUrl =
                        FileUpload.UploadImage(
                            vm.ImageFile,
                            _webHostEnvironment.WebRootPath);
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
            FileUpload.DeleteImage(
                _webHostEnvironment.WebRootPath,
                book.CoverImageUrl);

            _unitofWork.Book.Remove(book);
            _unitofWork.Save();

            _unitofWork.Book.DeleteBook(id);

            return RedirectToAction(nameof(Index));
        }
    }
}
