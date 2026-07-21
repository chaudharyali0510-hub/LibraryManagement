using LibraryManagement.Data;
using LibraryManagement.Models.Identity;
using Microsoft.EntityFrameworkCore;

namespace LibraryManagement.Services
{
    public class MenuService : IMenuService
    {
        private readonly ApplicationDbContext _context;

        public MenuService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<MenuItem>> GetUserMenusAsync(string userId)
        {
            var menus = await _context.UserRoles
                .Where(ur => ur.UserId == userId)
                .Join(
                    _context.RoleMenuItems,
                    ur => ur.RoleId,
                    rm => rm.RoleId,
                    (ur, rm) => rm.MenuItem
                )
                .Where(m => m.IsVisible)
                .OrderBy(m => m.SortOrder)
                .Distinct()
                .ToListAsync();

            return menus;
        }
    }
}
