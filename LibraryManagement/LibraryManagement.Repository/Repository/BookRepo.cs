using LibraryManagement.Data;
using LibraryManagement.Models;
using LibraryManagement.Repository.IRepository;
using LibraryManagement.ViewModel;
using Microsoft.EntityFrameworkCore;

namespace LibraryManagement.Repository
{
    public class BookRepo : Repository<Book>, IBookRepos
    {
        private readonly ApplicationDbContext _db;

        public BookRepo(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }

        public void Update(Book book)
        {
            _db.Books.Update(book);
        }

        public void DeleteBook(int id)
        {
            _db.Database.ExecuteSqlRaw(
                "EXEC sp_DeleteBook {0}",
                id
            );
        }

        public IQueryable<Book> GetFilteredBooks(BookFilterVM filter)
        {
            IQueryable<Book> query = _db.Books
                .AsNoTracking()
                .Include(b => b.Author)
                .Include(b => b.Publisher)
                .Include(b => b.BookGenres).ThenInclude(bg => bg.Genre)
                .Include(b => b.BookIssues).ThenInclude(bi => bi.Member)
                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(filter.Search))
            {
                var term = filter.Search.Trim().ToLower();
                query = query.Where(b =>
                    b.Title.ToLower().Contains(term) ||
                    b.ISBN.ToLower().Contains(term));
            }

            if (filter.AuthorId.HasValue && filter.AuthorId > 0)
                query = query.Where(b => b.AuthorId == filter.AuthorId.Value);

            if (filter.PublisherId.HasValue && filter.PublisherId > 0)
                query = query.Where(b => b.PublisherId == filter.PublisherId.Value);

            if (filter.GenreId.HasValue && filter.GenreId > 0)
                query = query.Where(b => b.BookGenres.Any(bg => bg.GenreId == filter.GenreId.Value));

            if (!string.IsNullOrEmpty(filter.Availability))
            {
                if (filter.Availability == "available")
                    query = query.Where(b => b.AvailableCopies > 0);
                else if (filter.Availability == "outofstock")
                    query = query.Where(b => b.AvailableCopies == 0);
            }

            query = filter.SortOrder switch
            {
                "title_desc" => query.OrderByDescending(b => b.Title),
                "year_desc" => query.OrderByDescending(b => b.PublicationYear),
                "year_asc" => query.OrderBy(b => b.PublicationYear),
                "copies_desc" => query.OrderByDescending(b => b.AvailableCopies),
                _ => query.OrderBy(b => b.Title)
            };

            return query;
        }

        public async Task<Book?> GetBookDetailsAsync(int id)
        {
            return await _db.Books
                .AsNoTracking()
                .Include(b => b.Author)
                .Include(b => b.Publisher)
                .Include(b => b.Series)
                .Include(b => b.BookGenres).ThenInclude(bg => bg.Genre)
                .Include(b => b.BookIssues).ThenInclude(bi => bi.Member)
                .FirstOrDefaultAsync(b => b.Id == id);
        }

        public async Task<List<Book>> GetSeriesBooksAsync(int seriesId, int excludeBookId)
        {
            return await _db.Books
                .AsNoTracking()
                .Include(b => b.Author)
                .Where(b => b.SeriesId == seriesId && b.Id != excludeBookId)
                .OrderBy(b => b.Title)
                .ToListAsync();
        }

        public async Task<IEnumerable<Book>> SearchBooksAsync(string query)
        {
            var term = query.Trim().ToLower();
            return await _db.Books
                .AsNoTracking()
                .Include(b => b.Author)
                .Include(b => b.Publisher)
                .Include(b => b.BookGenres).ThenInclude(bg => bg.Genre)
                .Where(b =>
                    b.Title.ToLower().Contains(term) ||
                    b.ISBN.ToLower().Contains(term) ||
                    b.Author!.FirstName.ToLower().Contains(term) ||
                    b.Author.LastName.ToLower().Contains(term) ||
                    (b.Author.FirstName + " " + b.Author.LastName).ToLower().Contains(term) ||
                    b.Publisher!.Name.ToLower().Contains(term) ||
                    b.BookGenres.Any(bg => bg.Genre.Name.ToLower().Contains(term)))
                .OrderBy(b => b.Title)
                .ToListAsync();
        }

        public async Task<IEnumerable<Book>> GetPopularBooksAsync(int count = 10)
        {
            return await _db.Books
                .AsNoTracking()
                .Include(b => b.Author)
                .Include(b => b.BookGenres).ThenInclude(bg => bg.Genre)
                .OrderByDescending(b => b.BookIssues.Count)
                .Take(count)
                .ToListAsync();
        }

        public async Task<IEnumerable<Book>> GetOverdueBooksAsync()
        {
            var today = DateTime.Today;
            return await _db.Books
                .AsNoTracking()
                .Include(b => b.Author)
                .Include(b => b.BookIssues).ThenInclude(bi => bi.Member)
                .Where(b => b.BookIssues.Any(bi => !bi.isReturned && bi.DueDate < today))
                .ToListAsync();
        }

        public async Task<IEnumerable<Book>> GetNewArrivalsAsync(int count = 10)
        {
            return await _db.Books
                .AsNoTracking()
                .Include(b => b.Author)
                .Include(b => b.Publisher)
                .Include(b => b.BookGenres).ThenInclude(bg => bg.Genre)
                .OrderByDescending(b => b.PublicationYear)
                .ThenByDescending(b => b.Id)
                .Take(count)
                .ToListAsync();
        }

        public async Task<IQueryable<Book>> GetAvailableBooksQueryAsync()
        {
            return _db.Books
                .AsNoTracking()
                .Include(b => b.Author)
                .Include(b => b.Publisher)
                .Include(b => b.BookGenres).ThenInclude(bg => bg.Genre)
                .Where(b => b.AvailableCopies > 0)
                .OrderBy(b => b.Title)
                .AsQueryable();
        }
    }
}
