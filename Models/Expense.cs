using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace jenkinsCICD.Models
{
    public class Expense
    {
        public int Id { get; set; }
        
        [Required]
        [StringLength(200)]
        public string Title { get; set; } = string.Empty;
        
        [StringLength(500)]
        public string? Description { get; set; }
        
        [Required]
        [Column(TypeName = "decimal(18,2)")]
        public decimal Amount { get; set; }
        
        [Required]
        public DateTime ExpenseDate { get; set; } = DateTime.Now;
        
        [Required]
        [StringLength(100)]
        public string Category { get; set; } = string.Empty;
        
        public int PaidByUserId { get; set; }
        public User PaidByUser { get; set; } = null!;
        
        public int? GroupId { get; set; }  // Nullable cho chi tiêu cá nhân
        public Group? Group { get; set; }
        
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        
        // Navigation properties
        public ICollection<ExpenseShare> ExpenseShares { get; set; } = new List<ExpenseShare>();
        
        // Computed properties
        public bool IsGroupExpense => GroupId.HasValue;
        public bool IsPersonalExpense => !GroupId.HasValue;
    }
}
