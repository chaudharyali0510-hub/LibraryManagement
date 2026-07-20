namespace LibraryManagement.Models
{
    public class Book
    {
        public int Id { get; set; }

        public string ISBN { get; set; }

        public string Title { get; set; }

        public string? Description { get; set; }

        public int AuthorId { get; set; }

        public Author? Author { get; set; }

        public int PublisherId { get; set; }

        public Publisher? Publisher { get; set; }

        public int PublicationYear { get; set; }

        public string? Edition { get; set; }

        public string? Language { get; set; }

        public int TotalCopies { get; set; }

        public int AvailableCopies { get; set; }

        public string? ShelfLocation { get; set; }

        public string? CoverImageUrl { get; set; }

        public bool IsAvailable { get; set; }

        public ICollection<BookGenre> BookGenres { get; set; }
            = new List<BookGenre>();

        public ICollection<BookIssue> BookIssues { get; set; }
            = new List<BookIssue>();
    }
}
