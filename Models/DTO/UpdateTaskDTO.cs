namespace TaskManagerAPI.Models.DTO
{
    public class UpdateTaskDTO
    {
        public string Title { get; set; }
        public string TaskDetails { get; set; }
        public DateTime Deadline { get; set; }
        public int Priority { get; set; }

    }
}
