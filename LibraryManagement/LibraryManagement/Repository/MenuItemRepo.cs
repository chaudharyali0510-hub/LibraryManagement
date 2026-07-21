using LibraryManagement.Data;
using LibraryManagement.Models.Identity;
using LibraryManagement.Repository.IRepository;

namespace LibraryManagement.Repository
{
    public class MenuItemRepo : Repository<MenuItem>, IMenuItemRepo
    {
        private readonly ApplicationDbContext _db;

        public MenuItemRepo(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }

        public void Update(MenuItem menuItem)
        {
            _db.MenuItems.Update(menuItem);
        }
    }
}
