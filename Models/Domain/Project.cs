namespace TaskManagerAPI.Models.Domain
{
    public class Project
    {
        public Guid ProjectId { get; set; }
        public string Title { get; set; }
        public Guid UserId { get; set; } // User who created the project

        public List<TaskModel> Tasks { get; set; } = new List<TaskModel>();

        public User User { get; set; }

    }
}
