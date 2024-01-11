using Microsoft.EntityFrameworkCore;
using TaskManagerAPI.Data;
using TaskManagerAPI.Models.Domain;

namespace TaskManagerAPI.Repositories
{
    public class SQLTaskRepository : ITaskRepository
    {
        private readonly TaskDbContext dbContext;

        public SQLTaskRepository(TaskDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task<TaskModel> CreateAsync(TaskModel taskModel)

        {
            var project = await dbContext.Projects.FindAsync(taskModel.ProjectId);
            if (project == null)
            {
                throw new Exception("Project not found");
            }


            // Check if the user who created the task exists
            var user = await dbContext.Users.FindAsync(taskModel.UserId);
            if (user == null)
            {
                throw new Exception("User not found");
            }
        
            await dbContext.Tasks.AddAsync(taskModel);
            await dbContext.SaveChangesAsync();

            var createdTaskWithStatusCategory = await dbContext.Tasks
                                                      .Include(t => t.StatusCategory)
                                                      .FirstOrDefaultAsync(t => t.TaskId == taskModel.TaskId);

            return createdTaskWithStatusCategory;

        }

        public async Task<TaskModel?> DeleteAsync(Guid id)
        {
            var existingTask = await dbContext.Tasks.FirstOrDefaultAsync(x => x.TaskId == id);

            if (existingTask == null)
            {
                return null;
            }

            dbContext.Tasks.Remove(existingTask);
            await dbContext.SaveChangesAsync();
            return existingTask;
        }

        public async Task<List<TaskModel>> GetAllAsync()
        {
            return await dbContext.Tasks
                 .Include(x => x.StatusCategory) // Include the StatusCategory in the query
                 .ToListAsync();
        }

        public async Task<TaskModel?> GetByIdAsync(Guid id)
        {
            return await dbContext.Tasks
            .Include(x => x.StatusCategory) 
            .FirstOrDefaultAsync(x => x.TaskId == id);
        }

        public async Task<TaskModel?> UpdateAsync(Guid id, TaskModel updatedTask)
        {
            var existingTask = await dbContext.Tasks.FirstOrDefaultAsync(x => x.TaskId == id);

            if (existingTask == null)
            {
                return null;
            }

            if (updatedTask.Title != null)
                existingTask.Title = updatedTask.Title;

            if (updatedTask.TaskDetails != null)
                existingTask.TaskDetails = updatedTask.TaskDetails;

            if (updatedTask.AssignedUserId != null) 
                existingTask.AssignedUserId = updatedTask.AssignedUserId;

            if (updatedTask.Deadline != default(DateTime)) // Check if a date is set
                existingTask.Deadline = updatedTask.Deadline;

            if (updatedTask.Priority != default(int)) // Assuming Priority is an int
                existingTask.Priority = updatedTask.Priority;

            await dbContext.SaveChangesAsync();
            return existingTask;
        }

        public async Task<TaskModel?> UpdateStatusAsync(Guid id, int newStatusCategoryId)
        {
            var existingTask = await dbContext.Tasks.FirstOrDefaultAsync(x => x.TaskId == id);

            if (existingTask == null)
            {
                return null;
            }

            existingTask.StatusCategoryId = newStatusCategoryId;
            await dbContext.SaveChangesAsync();
            return existingTask;
        }
    }
}

