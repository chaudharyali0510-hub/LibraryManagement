namespace LibraryManagement.Models.Identity
{
    public class MenuItem
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string Controller { get; set; } = null!;
        public string Action { get; set; } = null!;
        public string IconClass { get; set; } = null!;
        public int SortOrder { get; set; }
        public bool IsVisible { get; set; } = true;

        public ICollection<RoleMenuItem> RoleMenuItems { get; set; } = new List<RoleMenuItem>();
    }
}
