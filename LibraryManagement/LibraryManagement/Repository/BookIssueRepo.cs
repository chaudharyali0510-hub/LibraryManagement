using LibraryManagement.Data;
using LibraryManagement.Models;
using LibraryManagement.Repository.IRepository;


namespace LibraryManagement.Repository
{
    public class BookIssueRepo:Repository<BookIssue>,IBookIssueRepo
    {
        private readonly ApplicationDbContext _db;

        public BookIssueRepo(ApplicationDbContext db):base(db)
        {
            _db = db;
        }

        public void Update(BookIssue bookIssue)
        {
            _db.BookIssues.Update(bookIssue);
        }
    }
}
