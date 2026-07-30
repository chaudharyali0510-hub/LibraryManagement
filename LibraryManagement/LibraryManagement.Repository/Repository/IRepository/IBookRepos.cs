using LibraryManagement.Models;
using LibraryManagement.ViewModel;

namespace LibraryManagement.Repository.IRepository
{
    public interface IBookRepos : IRepository<Book>
    {
        void Update(Book book);
        void DeleteBook(int id);
        IQueryable<Book> GetFilteredBooks(BookFilterVM filter);
        Task<Book?> GetBookDetailsAsync(int id);
        Task<List<Book>> GetSeriesBooksAsync(int seriesId, int excludeBookId);
        Task<IEnumerable<Book>> SearchBooksAsync(string query);
        Task<IEnumerable<Book>> GetPopularBooksAsync(int count = 10);
        Task<IEnumerable<Book>> GetOverdueBooksAsync();
        Task<IEnumerable<Book>> GetNewArrivalsAsync(int count = 10);
        Task<IQueryable<Book>> GetAvailableBooksQueryAsync();
    }
}
