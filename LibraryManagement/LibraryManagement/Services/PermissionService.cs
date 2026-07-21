using LibraryManagement.Data;
using Microsoft.EntityFrameworkCore;

namespace LibraryManagement.Services
{
    public class PermissionService : IPermissionService
    {
        private readonly ApplicationDbContext _context;

        public PermissionService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<string>> GetUserPermissionsAsync(string userId)
        {
            var rolePermissions = await _context.UserRoles
                .Where(ur => ur.UserId == userId)
                .Join(
                    _context.RolePermissions,
                    ur => ur.RoleId,
                    rp => rp.RoleId,
                    (ur, rp) => rp.Permission.Name
                )
                .ToListAsync();

            var userPermissions = await _context.UserPermissions
                .Where(up => up.UserId == userId)
                .Select(up => up.Permission.Name)
                .ToListAsync();

            return rolePermissions.Concat(userPermissions)
                .Distinct()
                .ToList();
        }

        public async Task<bool> UserHasPermissionAsync(
            string userId,
            string permission)
        {
            var permissions = await GetUserPermissionsAsync(userId);
            return permissions.Contains(permission);
        }
    }
}