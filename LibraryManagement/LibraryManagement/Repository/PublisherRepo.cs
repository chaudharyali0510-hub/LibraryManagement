using LibraryManagement.Data;
using LibraryManagement.Models;
using LibraryManagement.Repository.IRepository;

namespace LibraryManagement.Repository
{
    public class PublisherRepo:Repository<Publisher>,IPublisherRepo
    {
        private readonly ApplicationDbContext _db;

        public PublisherRepo(ApplicationDbContext db):base(db)
        {
            _db = db;
        }

        public void Update(Publisher publisher)
        {
            _db.Publisher.Update(publisher);
        }
    }
}
