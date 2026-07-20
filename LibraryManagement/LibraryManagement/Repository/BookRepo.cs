using LibraryManagement.Data;
using LibraryManagement.Models;
using LibraryManagement.Repository.IRepository;
using Microsoft.EntityFrameworkCore;

namespace LibraryManagement.Repository
{
    public class BookRepo: Repository<Book>,IBookRepos
    {
        private readonly ApplicationDbContext _db;

        public BookRepo(ApplicationDbContext db) :base(db)
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
    }
}
