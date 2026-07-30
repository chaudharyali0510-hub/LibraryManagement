using AutoMapper;
using LibraryManagement.DTOs;
using LibraryManagement.Models;
using LibraryManagement.Repository.IRepository;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LibraryManagement.Controllers.Api
{
    [ApiController]
    [Route("api/books")]
    public class BooksController : ControllerBase
    {
        private readonly IUnitofWork _unitofWork;
        private readonly IMapper _mapper;

        public BooksController(IUnitofWork unitofWork, IMapper mapper)
        {
            _unitofWork = unitofWork;
            _mapper = mapper;
        }

        [HttpGet("search")]
        public async Task<IActionResult> Search(
            [FromQuery] string? query,
            [FromQuery] int page = 1,
            [FromQuery] int pageSize = 10)
        {
            if (string.IsNullOrWhiteSpace(query))
                return BadRequest(new { error = "Query parameter is required" });

            page = Math.Max(1, page);
            pageSize = Math.Clamp(pageSize, 1, 50);

            var books = await _unitofWork.Book.SearchBooksAsync(query);
            var totalRecords = books.Count();
            var pagedBooks = books
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            var result = new PagedResult<BookSearchResultDto>
            {
                Page = page,
                PageSize = pageSize,
                TotalRecords = totalRecords,
                Data = _mapper.Map<List<BookSearchResultDto>>(pagedBooks)
            };

            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var book = await _unitofWork.Book.GetBookDetailsAsync(id);
            if (book == null)
                return NotFound(new { error = $"Book with id {id} not found" });

            var result = _mapper.Map<BookDetailDto>(book);
            return Ok(result);
        }

        [HttpGet("available")]
        public async Task<IActionResult> GetAvailable(
            [FromQuery] int page = 1,
            [FromQuery] int pageSize = 10)
        {
            page = Math.Max(1, page);
            pageSize = Math.Clamp(pageSize, 1, 50);

            var query = await _unitofWork.Book.GetAvailableBooksQueryAsync();
            var totalRecords = await query.CountAsync();
            var books = await query
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            var result = new PagedResult<BookSearchResultDto>
            {
                Page = page,
                PageSize = pageSize,
                TotalRecords = totalRecords,
                Data = _mapper.Map<List<BookSearchResultDto>>(books)
            };

            return Ok(result);
        }

        [HttpGet("overdue")]
        public async Task<IActionResult> GetOverdue(
            [FromQuery] int page = 1,
            [FromQuery] int pageSize = 10)
        {
            page = Math.Max(1, page);
            pageSize = Math.Clamp(pageSize, 1, 50);

            var books = await _unitofWork.Book.GetOverdueBooksAsync();
            var overdueDtos = books.SelectMany(b =>
            {
                var overdueIssues = b.BookIssues
                    .Where(bi => !bi.isReturned && bi.DueDate < DateTime.Today)
                    .Select(bi => new OverdueBookDto
                    {
                        BookId = b.Id,
                        BookTitle = b.Title,
                        Borrower = $"{bi.Member!.FirstName} {bi.Member.LastName}",
                        IssueDate = bi.DateIssue,
                        DueDate = bi.DueDate,
                        DaysOverdue = (DateTime.Today - bi.DueDate).Days
                    });
                return overdueIssues;
            }).ToList();

            var totalRecords = overdueDtos.Count;
            var pagedData = overdueDtos
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            var result = new PagedResult<OverdueBookDto>
            {
                Page = page,
                PageSize = pageSize,
                TotalRecords = totalRecords,
                Data = pagedData
            };

            return Ok(result);
        }

        [HttpGet("popular")]
        public async Task<IActionResult> GetPopular(
            [FromQuery] int page = 1,
            [FromQuery] int pageSize = 10)
        {
            page = Math.Max(1, page);
            pageSize = Math.Clamp(pageSize, 1, 50);

            var books = await _unitofWork.Book.GetPopularBooksAsync(page * pageSize);
            var totalRecords = books.Count();
            var pagedBooks = books
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            var result = new PagedResult<PopularBookDto>
            {
                Page = page,
                PageSize = pageSize,
                TotalRecords = totalRecords,
                Data = _mapper.Map<List<PopularBookDto>>(pagedBooks)
            };

            return Ok(result);
        }

        [HttpGet("new")]
        public async Task<IActionResult> GetNewArrivals(
            [FromQuery] int page = 1,
            [FromQuery] int pageSize = 10)
        {
            page = Math.Max(1, page);
            pageSize = Math.Clamp(pageSize, 1, 50);

            var books = await _unitofWork.Book.GetNewArrivalsAsync(page * pageSize);
            var totalRecords = books.Count();
            var pagedBooks = books
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            var result = new PagedResult<BookSearchResultDto>
            {
                Page = page,
                PageSize = pageSize,
                TotalRecords = totalRecords,
                Data = _mapper.Map<List<BookSearchResultDto>>(pagedBooks)
            };

            return Ok(result);
        }

        [HttpGet("{id}/status")]
        public async Task<IActionResult> GetStatus(int id)
        {
            var book = await _unitofWork.Book.GetBookDetailsAsync(id);
            if (book == null)
                return NotFound(new { error = $"Book with id {id} not found" });

            var activeIssue = book.BookIssues
                .Where(bi => !bi.isReturned)
                .OrderByDescending(bi => bi.DateIssue)
                .FirstOrDefault();

            var status = new BookStatusDto
            {
                IsAvailable = book.AvailableCopies > 0,
                AvailableCopies = book.AvailableCopies,
                TotalCopies = book.TotalCopies,
                CurrentBorrower = activeIssue != null
                    ? $"{activeIssue.Member?.FirstName} {activeIssue.Member?.LastName}"
                    : null,
                IssueDate = activeIssue?.DateIssue,
                DueDate = activeIssue?.DueDate
            };

            return Ok(status);
        }
    }
}
