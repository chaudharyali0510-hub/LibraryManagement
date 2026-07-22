using LibraryManagement.Models;

namespace LibraryManagement.Repository.IRepository
{
    public interface IUnitofWork
    {
        IAuthorRepo Author { get; }
        IBookIssueRepo BookIssue { get; }
        IBookRepos Book {  get; }
        IGenreRepo Genre { get; }
        IMemberRepo Member { get; }
        IPublisherRepo Publisher { get; }
        IMenuItemRepo MenuItem { get; }
        void Save();
    }
}
