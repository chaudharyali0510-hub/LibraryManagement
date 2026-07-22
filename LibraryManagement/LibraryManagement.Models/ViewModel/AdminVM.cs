namespace LibraryManagement.ViewModel
{
    public class PermissionCheckboxVM
    {
        public int PermissionId { get; set; }
        public string PermissionName { get; set; } = null!;
        public string Module { get; set; } = null!;
        public bool IsSelected { get; set; }
    }

    public class RoleCheckboxVM
    {
        public string RoleId { get; set; } = null!;
        public string RoleName { get; set; } = null!;
        public bool IsSelected { get; set; }
    }

    public class MenuItemCheckboxVM
    {
        public int MenuItemId { get; set; }
        public string Name { get; set; } = null!;
        public bool IsSelected { get; set; }
    }

    public class RoleVM
    {
        public string Id { get; set; } = null!;
        public string Name { get; set; } = null!;
        public string Description { get; set; } = null!;
        public List<PermissionCheckboxVM> Permissions { get; set; } = new();
    }

    public class UserVM
    {
        public string UserId { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string FullName { get; set; } = null!;
        public List<RoleCheckboxVM> Roles { get; set; } = new();
        public List<PermissionCheckboxVM> DirectPermissions { get; set; } = new();
    }

    public class MenuConfigVM
    {
        public string RoleId { get; set; } = null!;
        public string RoleName { get; set; } = null!;
        public List<MenuItemCheckboxVM> MenuItems { get; set; } = new();
    }
}
