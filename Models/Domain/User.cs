namespace TaskManagerAPI.Models.Domain
{
    public class User
    {
        public Guid UserId { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string UserName { get; set; }
        public string Role { get; set; }


        public List<Project> Projects { get; set; } = new List<Project>();
        public List<TaskModel> AssignedTasks { get; set; } = new List<TaskModel>(); // Tasks assigned to the user
    }
}

