using System.ComponentModel.DataAnnotations;

namespace LibraryManagement.Models
{
    public class Series
    {
        public int Id { get; set; }

        [Required]
        public string Name { get; set; } = null!;

        public string? Description { get; set; }

        public ICollection<Book> Books { get; set; } = new List<Book>();
    }
}
