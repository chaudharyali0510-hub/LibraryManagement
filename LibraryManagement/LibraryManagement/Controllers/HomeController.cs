using LibraryManagement.Models;
using LibraryManagement.Repository.IRepository;
using LibraryManagement.ViewModel;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace LibraryManagement.Controllers
{
    public class HomeController : Controller
    {
        private readonly IUnitofWork _unitOfWork;

        public HomeController(IUnitofWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public IActionResult Index()
        {
            var vm = new DashboardVM
            {
                TotalBooks = _unitOfWork.Book.GetAll().Count(),
                TotalAuthors = _unitOfWork.Author.GetAll().Count(),
                TotalMembers = _unitOfWork.Member.GetAll().Count(),
                TotalPublishers = _unitOfWork.Publisher.GetAll().Count(),
                TotalGenres = _unitOfWork.Genre.GetAll().Count(),
                AvailableBooks = _unitOfWork.Book.GetAll().Sum(b => b.AvailableCopies),
                IssuedBooks = _unitOfWork.BookIssue.GetAll().Count(i => !i.isReturned),
                OverdueBooks = _unitOfWork.BookIssue.GetAll().Count(i => !i.isReturned && i.DueDate < DateTime.Now),
                TotalFines = _unitOfWork.BookIssue.GetAll().Where(i => i.FineAmount > 0).Sum(i => i.FineAmount),
                ActiveMembers = _unitOfWork.Member.GetAll().Count(m => m.isActive),
                RecentIssues = _unitOfWork.BookIssue.GetAll()
                    .OrderByDescending(i => i.DateIssue)
                    .Take(5)
                    .ToList(),
                OverdueList = _unitOfWork.BookIssue.GetAll()
                    .Where(i => !i.isReturned && i.DueDate < DateTime.Now)
                    .OrderBy(i => i.DueDate)
                    .Take(5)
                    .ToList()
            };

            return View(vm);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
