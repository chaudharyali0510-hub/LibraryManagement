namespace LibraryManagement.DTOs
{
    public class OverdueBookDto
    {
        public int BookId { get; set; }
        public string BookTitle { get; set; } = null!;
        public string Borrower { get; set; } = null!;
        public DateTime IssueDate { get; set; }
        public DateTime DueDate { get; set; }
        public int DaysOverdue { get; set; }
    }
}
