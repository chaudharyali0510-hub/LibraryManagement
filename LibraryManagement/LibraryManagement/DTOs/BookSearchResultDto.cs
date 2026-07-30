namespace LibraryManagement.DTOs
{
    public class BookSearchResultDto
    {
        public int Id { get; set; }
        public string Title { get; set; } = null!;
        public string ISBN { get; set; } = null!;
        public string Author { get; set; } = null!;
        public string Publisher { get; set; } = null!;
        public int AvailableCopies { get; set; }
        public int TotalCopies { get; set; }
        public string? ShelfLocation { get; set; }
        public string? Description { get; set; }
        public string? CoverImageUrl { get; set; }
        public List<string> Genres { get; set; } = new();
    }
}
