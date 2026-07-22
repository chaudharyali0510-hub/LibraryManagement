namespace LibraryManagement.Models
{
    public class Publisher
    {
        public int Id { get; set; }

        public string Name { get; set; } = null!;

        public string? Email { get; set; }

        public string? Phone { get; set; }

        public string? Website { get; set; }

        public string? Address { get; set; }

        public ICollection<Book> Books { get; set; } = new List<Book>();
    }
}
