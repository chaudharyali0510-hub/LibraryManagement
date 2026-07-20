using LibraryManagement.Models;

namespace LibraryManagement.Repository.IRepository
{
    public interface IPublisherRepo:IRepository<Publisher>
    {
        void Update(Publisher publisher);
    }
}
