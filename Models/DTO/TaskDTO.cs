using TaskManagerAPI.Models.Domain;

namespace TaskManagerAPI.Models.DTO
{
    public class TaskDTO
    {
        public Guid TaskId { get; set; }
        public string Title { get; set; }
        public string TaskDetails { get; set; }
        public DateTime Deadline { get; set; }
        public string UserName { get; set; }
        public int Priority { get; set; }
        public Guid AssignedUserId { get; set; }
        public Guid ProjectId { get; set; }
        public StatusCategoryDTO StatusCategory { get; set; }
        
    }
}
