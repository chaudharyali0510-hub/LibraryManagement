namespace LibraryManagement.DTOs
{
    public class PopularBookDto
    {
        public int Id { get; set; }
        public string Title { get; set; } = null!;
        public string Author { get; set; } = null!;
        public string? CoverImageUrl { get; set; }
        public int TotalIssues { get; set; }
    }
}
