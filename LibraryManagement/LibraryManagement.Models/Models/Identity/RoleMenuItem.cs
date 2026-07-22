namespace LibraryManagement.Models.Identity
{
    public class RoleMenuItem
    {
        public string RoleId { get; set; } = null!;
        public ApplicationRole Role { get; set; } = null!;
        public int MenuItemId { get; set; }
        public MenuItem MenuItem { get; set; } = null!;
    }
}
