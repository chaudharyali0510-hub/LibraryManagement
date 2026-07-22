using LibraryManagement.Models;

namespace LibraryManagement.ViewModel
{
    public class DashboardVM
    {
        public int TotalBooks { get; set; }
        public int TotalAuthors { get; set; }
        public int TotalMembers { get; set; }
        public int TotalPublishers { get; set; }
        public int TotalGenres { get; set; }
        public int IssuedBooks { get; set; }
        public int AvailableBooks { get; set; }
        public int OverdueBooks { get; set; }
        public decimal TotalFines { get; set; }
        public int ActiveMembers { get; set; }
        public List<BookIssue> RecentIssues { get; set; } = new();
        public List<BookIssue> OverdueList { get; set; } = new();
    }
}
