namespace jenkinsCICD.Models
{
    public class GroupMember
    {
        public int Id { get; set; }
        
        public int GroupId { get; set; }
        public Group Group { get; set; } = null!;
        
        public int UserId { get; set; }
        public User User { get; set; } = null!;
        
        public DateTime JoinedAt { get; set; } = DateTime.Now;
        public bool IsAdmin { get; set; } = false;
        public bool IsActive { get; set; } = true;
    }
}
