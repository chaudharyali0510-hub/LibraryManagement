using LibraryManagement.Models.Identity;

namespace LibraryManagement.Services
{
    public interface IMenuService
    {
        Task<List<MenuItem>> GetUserMenusAsync(string userId);
    }
}
