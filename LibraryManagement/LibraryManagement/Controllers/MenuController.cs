using LibraryManagement.Data;
using LibraryManagement.Models.Identity;
using LibraryManagement.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LibraryManagement.Controllers
{
    [Authorize(Policy = "Menus.View")]
    public class MenuController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly RoleManager<ApplicationRole> _roleManager;

        public MenuController(
            ApplicationDbContext context,
            RoleManager<ApplicationRole> roleManager)
        {
            _context = context;
            _roleManager = roleManager;
        }

        public async Task<IActionResult> Index()
        {
            var menuItems = await _context.MenuItems
                .OrderBy(m => m.SortOrder)
                .ToListAsync();
            return View(menuItems);
        }

        public IActionResult Upsert(int? id)
        {
            var menuItem = new MenuItem();
            if (id != null && id != 0)
            {
                menuItem = _context.MenuItems.Find(id);
                if (menuItem == null) return NotFound();
            }
            return View(menuItem);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Upsert(MenuItem menuItem)
        {
            if (menuItem.Id == 0)
            {
                _context.MenuItems.Add(menuItem);
            }
            else
            {
                _context.MenuItems.Update(menuItem);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            var menuItem = await _context.MenuItems.FindAsync(id);
            if (menuItem == null) return NotFound();

            var roleMenuItems = _context.RoleMenuItems
                .Where(rm => rm.MenuItemId == id)
                .ToList();
            _context.RoleMenuItems.RemoveRange(roleMenuItems);

            _context.MenuItems.Remove(menuItem);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Assign(string roleId)
        {
            var role = await _roleManager.FindByIdAsync(roleId);
            if (role == null) return NotFound();

            var roleMenuItemIds = _context.RoleMenuItems
                .Where(rm => rm.RoleId == roleId)
                .Select(rm => rm.MenuItemId)
                .ToList();

            var menuItems = await _context.MenuItems
                .OrderBy(m => m.SortOrder)
                .Select(m => new MenuItemCheckboxVM
                {
                    MenuItemId = m.Id,
                    Name = m.Name,
                    IsSelected = roleMenuItemIds.Contains(m.Id)
                })
                .ToListAsync();

            var vm = new MenuConfigVM
            {
                RoleId = roleId,
                RoleName = role.Name,
                MenuItems = menuItems
            };

            return View(vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Assign(MenuConfigVM vm)
        {
            var existing = _context.RoleMenuItems
                .Where(rm => rm.RoleId == vm.RoleId)
                .ToList();

            _context.RoleMenuItems.RemoveRange(existing);

            foreach (var item in vm.MenuItems.Where(m => m.IsSelected))
            {
                _context.RoleMenuItems.Add(new RoleMenuItem
                {
                    RoleId = vm.RoleId,
                    MenuItemId = item.MenuItemId
                });
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
    }
}
