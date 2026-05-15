namespace ClientProjectTracker.Models
{
    public class TaskItem
    {
        public int Id { get; set; }

        public required string Title { get; set; }
        public string? Description { get; set; }

        public string Status { get; set; } = "Pending"; // Pending / In Progress / Done

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        // 🔗 Relationship
        public int ProjectId { get; set; }
        public Project? Project { get; set; }
    }
}