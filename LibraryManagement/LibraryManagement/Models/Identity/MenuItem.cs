namespace LibraryManagement.Models.Identity
{
    public class MenuItem
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Controller { get; set; }
        public string Action { get; set; }
        public string IconClass { get; set; }
        public int SortOrder { get; set; }
        public bool IsVisible { get; set; } = true;

        public ICollection<RoleMenuItem> RoleMenuItems { get; set; } = new List<RoleMenuItem>();
    }
}
