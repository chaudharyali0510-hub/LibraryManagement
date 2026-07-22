using LibraryManagement.Data;
using LibraryManagement.Models;
using LibraryManagement.Repository.IRepository;

namespace LibraryManagement.Repository
{
    public class GenreRepo:Repository<Genre>,IGenreRepo
    {
        private readonly ApplicationDbContext _db;

        public GenreRepo(ApplicationDbContext db):base(db) 
        {
            _db = db;
        }

        public void Update(Genre genre)
        {
            _db.Genres.Update(genre);
        }
    }
}
