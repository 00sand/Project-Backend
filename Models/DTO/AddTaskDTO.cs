using TaskManagerAPI.Models.Domain;

namespace TaskManagerAPI.Models.DTO
{
    public class AddTaskDTO
    {
        public string Title { get; set; }
        public string TaskDetails { get; set; }
        public DateTime Deadline { get; set; }
        public int Priority { get; set; }
        public string UserId { get; set; }
        public Guid ProjectId { get; set; }

        public string AssignedUserId { get; set; }


    }
}
