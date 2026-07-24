using LibraryManagement.Data;
using LibraryManagement.Models;
using LibraryManagement.Repository.IRepository;

namespace LibraryManagement.Repository
{
    public class SeriesRepo : Repository<Series>, ISeriesRepo
    {
        private readonly ApplicationDbContext _db;

        public SeriesRepo(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }

        public void Update(Series series)
        {
            _db.Series.Update(series);
        }
    }
}
