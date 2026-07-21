namespace LibraryManagement.Models.Identity
{
    public class Permission
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Module { get; set; }

        public string? Description { get; set; }


        public ICollection<RolePermission> RolePermissions { get; set; } = new List<RolePermission>();


        public ICollection<UserPermission> UserPermissions { get; set; } = new List<UserPermission>();
    }
}