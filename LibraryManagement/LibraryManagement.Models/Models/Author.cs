using System.ComponentModel.DataAnnotations;

namespace LibraryManagement.Models
{
    public class Author
    {
        [Key]
        public int Id { get; set; }

        public string FirstName { get; set; } = null!;

        public string LastName { get; set; } = null!;

        public string? Biography { get; set; }

        public DateTime? DateOfBirth { get; set; }

        public string? Nationality { get; set; }

        public ICollection<Book> Books { get; set; } = new List<Book>();
    }
}
