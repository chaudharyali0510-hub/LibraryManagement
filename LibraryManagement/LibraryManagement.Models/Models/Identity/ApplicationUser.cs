using Microsoft.AspNetCore.Identity;

namespace LibraryManagement.Models.Identity
{
    public class ApplicationUser : IdentityUser
    {
        public string FullName { get; set; } = null!;
    }
}