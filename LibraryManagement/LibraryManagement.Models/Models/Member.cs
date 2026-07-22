using System.ComponentModel.DataAnnotations;

namespace LibraryManagement.Models
{
    public class Member
    {
        [Key]
        public int Id { get; set;  }
        public string FirstName { get; set; } = null!;
        public string LastName { get; set; } = null!;
        [EmailAddress]
        public string Email { get; set; } = null!;
        public string PhoneNumber { get; set; } = null!;
        public string Address { get; set; } = null!;
        public DateTime MembershipDate { get; set; }= DateTime.Now;
        public bool isActive { get; set; } = true;

        public ICollection<BookIssue> BookIssues { get; set; }
        = new List<BookIssue>();
    }
}
