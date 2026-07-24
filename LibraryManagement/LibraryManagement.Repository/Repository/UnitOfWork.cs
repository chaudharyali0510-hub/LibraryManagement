using LibraryManagement.Data;
using LibraryManagement.Repository.IRepository;

namespace LibraryManagement.Repository
{
    public class UnitOfWork:IUnitofWork
    {
        private readonly ApplicationDbContext _db;

        public UnitOfWork(ApplicationDbContext db)
        {
            _db = db;
            Author = new AuthorRepo(_db);
            BookIssue = new BookIssueRepo(_db);
            Book = new BookRepo(_db);
            Genre = new GenreRepo(_db);
            Member = new MemberRepo(_db);
            Publisher = new PublisherRepo(_db);
            MenuItem = new MenuItemRepo(_db);
            Series = new SeriesRepo(_db);
        }

        public IAuthorRepo Author {  get; private set; }

        public IBookIssueRepo BookIssue { get; private set; }

        public IBookRepos Book { get; private set; }

        public IGenreRepo Genre { get; private set; }

        public IMemberRepo Member { get; private set; }

        public IPublisherRepo Publisher { get; private set; }

        public IMenuItemRepo MenuItem { get; private set; }

        public ISeriesRepo Series { get; private set; }

        public void Save()
        {
            _db.SaveChanges();
        }
    }
}
