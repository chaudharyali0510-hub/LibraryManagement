using LibraryManagement.Models;

namespace LibraryManagement.Repository.IRepository
{
    public interface IAuthorRepo:IRepository<Author>
    {
        void Update(Author author);
    }
}
