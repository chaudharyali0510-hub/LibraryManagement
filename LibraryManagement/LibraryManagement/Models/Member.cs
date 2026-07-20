using System.ComponentModel.DataAnnotations;

namespace LibraryManagement.Models
{
    public class Member
    {
        [Key]
        public int Id { get; set;  }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        [EmailAddress]
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string Address { get; set; }
        public DateTime MembershipDate { get; set; }= DateTime.Now;
        public bool isActive { get; set; } = true;

        public ICollection<BookIssue> BookIssues { get; set; }
        = new List<BookIssue>();
    }
}
