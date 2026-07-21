using LibraryManagement.Data;
using LibraryManagement.Models.Identity;
using LibraryManagement.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LibraryManagement.Controllers
{
    [Authorize(Policy = "Roles.View")]
    public class RoleController : Controller
    {
        private readonly RoleManager<ApplicationRole> _roleManager;
        private readonly ApplicationDbContext _context;

        public RoleController(
            RoleManager<ApplicationRole> roleManager,
            ApplicationDbContext context)
        {
            _roleManager = roleManager;
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var roles = await _roleManager.Roles.ToListAsync();
            return View(roles);
        }

        public async Task<IActionResult> Upsert(string? id)
        {
            var vm = new RoleVM();

            if (!string.IsNullOrEmpty(id))
            {
                var role = await _roleManager.FindByIdAsync(id);
                if (role == null) return NotFound();

                vm.Id = role.Id;
                vm.Name = role.Name;
                vm.Description = role.Description;

                var rolePermissionIds = _context.RolePermissions
                    .Where(rp => rp.RoleId == id)
                    .Select(rp => rp.PermissionId)
                    .ToList();

                vm.Permissions = await _context.Permissions.Select(p => new PermissionCheckboxVM
                {
                    PermissionId = p.Id,
                    PermissionName = p.Name,
                    Module = p.Module,
                    IsSelected = rolePermissionIds.Contains(p.Id)
                }).ToListAsync();
            }
            else
            {
                vm.Permissions = await _context.Permissions.Select(p => new PermissionCheckboxVM
                {
                    PermissionId = p.Id,
                    PermissionName = p.Name,
                    Module = p.Module,
                    IsSelected = false
                }).ToListAsync();
            }

            return View(vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Upsert(RoleVM vm)
        {
            if (string.IsNullOrEmpty(vm.Id))
            {
                var role = new ApplicationRole
                {
                    Name = vm.Name,
                    NormalizedName = vm.Name.ToUpper(),
                    Description = vm.Description
                };

                var result = await _roleManager.CreateAsync(role);
                if (!result.Succeeded)
                {
                    foreach (var error in result.Errors)
                        ModelState.AddModelError("", error.Description);
                    vm.Permissions = await GetPermissionCheckboxes(null);
                    return View(vm);
                }

                vm.Id = role.Id;
            }
            else
            {
                var role = await _roleManager.FindByIdAsync(vm.Id);
                if (role == null) return NotFound();

                role.Name = vm.Name;
                role.NormalizedName = vm.Name.ToUpper();
                role.Description = vm.Description;
                await _roleManager.UpdateAsync(role);
            }

            var selectedPermissions = vm.Permissions
                .Where(p => p.IsSelected)
                .Select(p => p.PermissionId)
                .ToList();

            var existingRolePermissions = _context.RolePermissions
                .Where(rp => rp.RoleId == vm.Id)
                .ToList();

            _context.RolePermissions.RemoveRange(existingRolePermissions);

            foreach (var permId in selectedPermissions)
            {
                _context.RolePermissions.Add(new RolePermission
                {
                    RoleId = vm.Id,
                    PermissionId = permId
                });
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(string id)
        {
            var role = await _roleManager.FindByIdAsync(id);
            if (role == null) return NotFound();

            var rolePermissions = _context.RolePermissions
                .Where(rp => rp.RoleId == id)
                .ToList();
            _context.RolePermissions.RemoveRange(rolePermissions);

            var roleMenuItems = _context.RoleMenuItems
                .Where(rm => rm.RoleId == id)
                .ToList();
            _context.RoleMenuItems.RemoveRange(roleMenuItems);

            await _roleManager.DeleteAsync(role);
            return RedirectToAction(nameof(Index));
        }

        private async Task<List<PermissionCheckboxVM>> GetPermissionCheckboxes(string? roleId)
        {
            var rolePermissionIds = roleId != null
                ? _context.RolePermissions.Where(rp => rp.RoleId == roleId).Select(rp => rp.PermissionId).ToList()
                : new List<int>();

            return await _context.Permissions.Select(p => new PermissionCheckboxVM
            {
                PermissionId = p.Id,
                PermissionName = p.Name,
                Module = p.Module,
                IsSelected = rolePermissionIds.Contains(p.Id)
            }).ToListAsync();
        }
    }
}
