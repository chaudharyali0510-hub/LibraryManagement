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
            var menus = await (
                from ur in _context.UserRoles
                join rm in _context.RoleMenuItems on ur.RoleId equals rm.RoleId
                join m in _context.MenuItems on rm.MenuItemId equals m.Id
                where ur.UserId == userId && m.IsVisible
                
                select m
            ).Distinct().OrderBy(m=>m.SortOrder).ToListAsync();

            return menus;
        }
    }
}
