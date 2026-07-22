namespace LibraryManagement.Models.Identity
{
    public class UserPermission
    {
        public string UserId { get; set; } = null!;

        public ApplicationUser User { get; set; } = null!;


        public int PermissionId { get; set; }

        public Permission Permission { get; set; } = null!;
    }
}