namespace LibraryManagement.ViewModel
{
    public class PermissionCheckboxVM
    {
        public int PermissionId { get; set; }
        public string PermissionName { get; set; }
        public string Module { get; set; }
        public bool IsSelected { get; set; }
    }

    public class RoleCheckboxVM
    {
        public string RoleId { get; set; }
        public string RoleName { get; set; }
        public bool IsSelected { get; set; }
    }

    public class MenuItemCheckboxVM
    {
        public int MenuItemId { get; set; }
        public string Name { get; set; }
        public bool IsSelected { get; set; }
    }

    public class RoleVM
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public List<PermissionCheckboxVM> Permissions { get; set; } = new();
    }

    public class UserVM
    {
        public string UserId { get; set; }
        public string Email { get; set; }
        public string FullName { get; set; }
        public List<RoleCheckboxVM> Roles { get; set; } = new();
        public List<PermissionCheckboxVM> DirectPermissions { get; set; } = new();
    }

    public class MenuConfigVM
    {
        public string RoleId { get; set; }
        public string RoleName { get; set; }
        public List<MenuItemCheckboxVM> MenuItems { get; set; } = new();
    }
}
