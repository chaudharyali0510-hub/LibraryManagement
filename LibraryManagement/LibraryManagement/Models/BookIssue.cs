using System.ComponentModel.DataAnnotations;

namespace LibraryManagement.Models
{
    public class BookIssue
    {
        [Key]
        public int Id { get; set; }
        public int BookId { get; set; }
        public Book? Book { get; set; }
        public int MemberId { get; set; }
        public Member? Member { get; set; }
        public DateTime DateIssue { get; set; }
        public DateTime DueDate {  get; set; }
        public DateTime? ReturnDate { get; set; }
        public decimal FineAmount { get; set; }
        public bool isReturned { get; set; } = false;

    }
}
