using LibraryManagement.Models;

namespace LibraryManagement.Repository.IRepository
{
    public interface IBookIssueRepo:IRepository<BookIssue>
    {
        void Update(BookIssue bookIssue);
    }
}
