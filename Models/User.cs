using System.ComponentModel.DataAnnotations;

namespace jenkinsCICD.Models
{
    public class User
    {
        public int Id { get; set; }
        
        [Required]
        [StringLength(100)]
        public string Name { get; set; } = string.Empty;
        
        [Required]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;
        
        [Phone]
        public string? PhoneNumber { get; set; }
        
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        
        // Navigation properties
        public ICollection<Expense> Expenses { get; set; } = new List<Expense>();
        public ICollection<GroupMember> GroupMemberships { get; set; } = new List<GroupMember>();
        public ICollection<ExpenseShare> ExpenseShares { get; set; } = new List<ExpenseShare>();
    }
}
