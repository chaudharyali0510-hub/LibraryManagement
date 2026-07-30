using AutoMapper;
using LibraryManagement.DTOs;
using LibraryManagement.Models;

namespace LibraryManagement.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Book, BookSearchResultDto>()
                .ForMember(d => d.Author, o => o.MapFrom(
                    s => $"{s.Author!.FirstName} {s.Author.LastName}"))
                .ForMember(d => d.Publisher, o => o.MapFrom(
                    s => s.Publisher!.Name))
                .ForMember(d => d.Genres, o => o.MapFrom(
                    s => s.BookGenres.Select(bg => bg.Genre.Name).ToList()));

            CreateMap<Book, BookDetailDto>()
                .ForMember(d => d.Author, o => o.MapFrom(
                    s => $"{s.Author!.FirstName} {s.Author.LastName}"))
                .ForMember(d => d.AuthorBiography, o => o.MapFrom(
                    s => s.Author!.Biography))
                .ForMember(d => d.Publisher, o => o.MapFrom(
                    s => s.Publisher!.Name))
                .ForMember(d => d.Series, o => o.MapFrom(
                    s => s.Series != null ? s.Series.Name : null))
                .ForMember(d => d.Genres, o => o.MapFrom(
                    s => s.BookGenres.Select(bg => bg.Genre.Name).ToList()));

            CreateMap<Book, PopularBookDto>()
                .ForMember(d => d.Author, o => o.MapFrom(
                    s => $"{s.Author!.FirstName} {s.Author.LastName}"))
                .ForMember(d => d.TotalIssues, o => o.MapFrom(
                    s => s.BookIssues.Count));

            CreateMap<Author, AuthorDto>()
                .ForMember(d => d.BookCount, o => o.MapFrom(
                    s => s.Books.Count));

            CreateMap<Genre, GenreDto>();

            CreateMap<Publisher, PublisherDto>();
        }
    }
}
