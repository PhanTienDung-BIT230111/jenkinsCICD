using System.ComponentModel.DataAnnotations;

namespace jenkinsCICD.Models
{
    public class Group
    {
        public int Id { get; set; }
        
        [Required]
        [StringLength(200)]
        public string Name { get; set; } = string.Empty;
        
        [StringLength(500)]
        public string? Description { get; set; }
        
        public int CreatedByUserId { get; set; }
        public User CreatedByUser { get; set; } = null!;
        
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public bool IsActive { get; set; } = true;
        
        // Navigation properties
        public ICollection<GroupMember> Members { get; set; } = new List<GroupMember>();
        public ICollection<Expense> Expenses { get; set; } = new List<Expense>();
    }
}
