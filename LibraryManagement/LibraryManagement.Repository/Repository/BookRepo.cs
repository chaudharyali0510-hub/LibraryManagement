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
    }
}
