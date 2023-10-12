namespace TaskManagerAPI.Models.Domain
{
    public class TaskModel
    {
        public Guid TaskId { get; set; }
        public string Title { get; set; }
        public string TaskDetails { get; set; }
        public DateTime Deadline { get; set; }
        public int Priority { get; set; }
        public int StatusCategoryId { get; set; }
        public Guid UserId { get; set; } // User who created the task
        public string UserName { get; set; }

        public Guid ProjectId { get; set; }

        public Guid AssignedUserId { get; set; }


        public StatusCategory StatusCategory { get; set; }
       
    }
}
