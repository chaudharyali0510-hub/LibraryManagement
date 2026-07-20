using LibraryManagement.Models;

namespace LibraryManagement.Repository.IRepository
{
    public interface IBookRepos:IRepository<Book>
    {
        void Update(Book book);
        void DeleteBook(int id);
    }
}
