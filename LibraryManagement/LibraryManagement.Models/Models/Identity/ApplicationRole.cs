using Microsoft.AspNetCore.Identity;

namespace LibraryManagement.Models.Identity
{
    public class ApplicationRole : IdentityRole
    {
        public string Description { get; set; } = null!;
    }
}