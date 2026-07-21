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
            var rolePermissions = await (
                from ur in _context.UserRoles
                join rp in _context.RolePermissions on ur.RoleId equals rp.RoleId
                join p in _context.Permissions on rp.PermissionId equals p.Id
                where ur.UserId == userId
                select p.Name
            ).ToListAsync();

            var userPermissions = await (
                from up in _context.UserPermissions
                join p in _context.Permissions on up.PermissionId equals p.Id
                where up.UserId == userId
                select p.Name
            ).ToListAsync();

            return rolePermissions.Concat(userPermissions)
                .Distinct(StringComparer.OrdinalIgnoreCase)
                .ToList();
        }

        public async Task<bool> UserHasPermissionAsync(
            string userId,
            string permission)
        {
            var hasRolePermission = await (
                from ur in _context.UserRoles
                join rp in _context.RolePermissions on ur.RoleId equals rp.RoleId
                join p in _context.Permissions on rp.PermissionId equals p.Id
                where ur.UserId == userId && p.Name == permission
                select p.Id
            ).AnyAsync();

            if (hasRolePermission)
                return true;

            var hasUserPermission = await (
                from up in _context.UserPermissions
                join p in _context.Permissions on up.PermissionId equals p.Id
                where up.UserId == userId && p.Name == permission
                select p.Id
            ).AnyAsync();

            return hasUserPermission;
        }
    }
}