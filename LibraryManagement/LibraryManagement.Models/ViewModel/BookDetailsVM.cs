using LibraryManagement.Models;

namespace LibraryManagement.ViewModel
{
    public class BookDetailsVM
    {
        public Book Book { get; set; } = new();
        public List<BookIssue> ActiveIssues { get; set; } = new();
        public List<BookIssue> IssueHistory { get; set; } = new();
        public List<Book> SeriesBooks { get; set; } = new();
    }
}
