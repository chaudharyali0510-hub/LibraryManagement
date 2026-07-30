namespace LibraryManagement.DTOs
{
    public class BookDetailDto
    {
        public int Id { get; set; }
        public string Title { get; set; } = null!;
        public string ISBN { get; set; } = null!;
        public string Author { get; set; } = null!;
        public string? AuthorBiography { get; set; }
        public string Publisher { get; set; } = null!;
        public List<string> Genres { get; set; } = new();
        public string? Series { get; set; }
        public int PublicationYear { get; set; }
        public string? Edition { get; set; }
        public string? Language { get; set; }
        public int AvailableCopies { get; set; }
        public int TotalCopies { get; set; }
        public string? ShelfLocation { get; set; }
        public string? Description { get; set; }
        public string? CoverImageUrl { get; set; }
    }
}
