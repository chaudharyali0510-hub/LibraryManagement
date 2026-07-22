using LibraryManagement.Models.Identity;

namespace LibraryManagement.Repository.IRepository
{
    public interface IMenuItemRepo : IRepository<MenuItem>
    {
        void Update(MenuItem menuItem);
    }
}
