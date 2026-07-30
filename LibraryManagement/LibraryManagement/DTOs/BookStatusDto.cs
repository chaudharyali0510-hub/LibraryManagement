namespace LibraryManagement.DTOs
{
    public class BookStatusDto
    {
        public bool IsAvailable { get; set; }
        public string? CurrentBorrower { get; set; }
        public DateTime? IssueDate { get; set; }
        public DateTime? DueDate { get; set; }
        public int AvailableCopies { get; set; }
        public int TotalCopies { get; set; }
    }
}
