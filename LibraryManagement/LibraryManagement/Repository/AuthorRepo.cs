using LibraryManagement.Data;
using LibraryManagement.Models;
using LibraryManagement.Repository.IRepository;

namespace LibraryManagement.Repository
{
    public class AuthorRepo:Repository<Author>,IAuthorRepo
    {
        private readonly ApplicationDbContext _db;

        public AuthorRepo(ApplicationDbContext db):base(db)
        {
            _db = db;
        }

        public void Update(Author author)
        {
            _db.Author.Update(author);
        }
    }
}
