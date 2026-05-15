namespace ClientProjectTracker.Models
{
    public class Project
    {
        public int Id { get; set; }

        public required string Name { get; set; }
        public required string Description { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        // 🔗 Relationship
        public int ClientId { get; set; }
        public Client? Client { get; set; }
        public ICollection<TaskItem> Tasks { get; set; } = new List<TaskItem>();
    }
}