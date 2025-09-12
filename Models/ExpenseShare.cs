using System.ComponentModel.DataAnnotations.Schema;

namespace jenkinsCICD.Models
{
    public class ExpenseShare
    {
        public int Id { get; set; }
        
        public int ExpenseId { get; set; }
        public Expense Expense { get; set; } = null!;
        
        public int UserId { get; set; }
        public User User { get; set; } = null!;
        
        [Column(TypeName = "decimal(18,2)")]
        public decimal ShareAmount { get; set; }
        
        [Column(TypeName = "decimal(5,2)")]
        public decimal SharePercentage { get; set; } = 0;
        
        public bool IsPaid { get; set; } = false;
        public DateTime? PaidDate { get; set; }
        
        public DateTime CreatedAt { get; set; } = DateTime.Now;
    }
}
