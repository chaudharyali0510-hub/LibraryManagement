using Microsoft.AspNetCore.Mvc.Rendering;

namespace LibraryManagement.ViewModel
{
    public class BookFilterVM
    {
        public string? Search { get; set; }
        public int? AuthorId { get; set; }
        public int? PublisherId { get; set; }
        public int? GenreId { get; set; }
        public string? Availability { get; set; }
        public string? SortOrder { get; set; }
        public int Page { get; set; } = 1;
        public int PageSize { get; set; } = 9;

        public SelectList? Authors { get; set; }
        public SelectList? Publishers { get; set; }
        public SelectList? Genres { get; set; }
    }
}
