namespace TaskManagerAPI.Models.DTO
{
    public class ProjectDTO
    {
        public Guid ProjectId { get; set; }
        public string Title { get; set; }
        public Guid UserId { get; set; } // User who created the project

        public List<TaskDTO> Tasks { get; set; }
    }
}
