using LibraryManagement.Data;
using LibraryManagement.Models.Identity;
using LibraryManagement.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LibraryManagement.Controllers
{
    [Authorize(Policy = "Users.View")]
    public class UserController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<ApplicationRole> _roleManager;
        private readonly ApplicationDbContext _context;

        public UserController(
            UserManager<ApplicationUser> userManager,
            RoleManager<ApplicationRole> roleManager,
            ApplicationDbContext context)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var users = await _userManager.Users.ToListAsync();
            return View(users);
        }

        public async Task<IActionResult> Manage(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null) return NotFound();

            var userRoles = await _userManager.GetRolesAsync(user);

            var allRoles = await _roleManager.Roles.ToListAsync();
            var roles = allRoles.Select(r => new RoleCheckboxVM
            {
                RoleId = r.Id,
                RoleName = r.Name,
                IsSelected = userRoles.Contains(r.Name)
            }).ToList();

            var userPermissionIds = _context.UserPermissions
                .Where(up => up.UserId == userId)
                .Select(up => up.PermissionId)
                .ToList();

            var permissions = await _context.Permissions.Select(p => new PermissionCheckboxVM
            {
                PermissionId = p.Id,
                PermissionName = p.Name,
                Module = p.Module,
                IsSelected = userPermissionIds.Contains(p.Id)
            }).ToListAsync();

            var vm = new UserVM
            {
                UserId = user.Id,
                Email = user.Email,
                FullName = user.FullName,
                Roles = roles,
                DirectPermissions = permissions
            };

            return View(vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Manage(UserVM vm)
        {
            var user = await _userManager.FindByIdAsync(vm.UserId);
            if (user == null) return NotFound();

            foreach (var role in vm.Roles)
            {
                if (role.IsSelected)
                    await _userManager.AddToRoleAsync(user, role.RoleName);
                else
                    await _userManager.RemoveFromRoleAsync(user, role.RoleName);
            }

            var selectedPermissionIds = vm.DirectPermissions
                .Where(p => p.IsSelected)
                .Select(p => p.PermissionId)
                .ToList();

            var existingUserPermissions = _context.UserPermissions
                .Where(up => up.UserId == vm.UserId)
                .ToList();

            _context.UserPermissions.RemoveRange(existingUserPermissions);

            foreach (var permId in selectedPermissionIds)
            {
                _context.UserPermissions.Add(new UserPermission
                {
                    UserId = vm.UserId,
                    PermissionId = permId
                });
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
    }
}
