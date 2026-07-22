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
    }
}
