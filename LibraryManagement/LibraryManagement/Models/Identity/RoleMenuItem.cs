namespace LibraryManagement.Models.Identity
{
    public class RoleMenuItem
    {
        public string RoleId { get; set; }
        public ApplicationRole Role { get; set; }
        public int MenuItemId { get; set; }
        public MenuItem MenuItem { get; set; }
    }
}
