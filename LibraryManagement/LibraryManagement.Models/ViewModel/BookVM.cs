using LibraryManagement.Models;
using Microsoft.AspNetCore.Http;

namespace LibraryManagement.ViewModel
{
    public class BookVM
    {
        public IEnumerable<Book> Books { get; set; } = new List<Book>();

        public Book Book { get; set; } = new();

        public IEnumerable<Author> AuthorList { get; set; } = new List<Author>();

        public IEnumerable<Publisher> PublisherList { get; set; } = new List<Publisher>();

        public IEnumerable<Genre> GenreList { get; set; } = new List<Genre>();

        public List<int> SelectedGenreIds { get; set; } = new();
        public IFormFile? ImageFile { get; set; }
        public IEnumerable<Series> SeriesList { get; set; } = new List<Series>();
        public int? SelectedSeriesId { get; set; }
    }
}