using LibraryManagement.Models;

namespace LibraryManagement.Repository.IRepository
{
    public interface IGenreRepo:IRepository<Genre>
    {
        void Update(Genre genre);
    }
}
