using System.Runtime.InteropServices;
using TaskManagerAPI.Models.Domain;

namespace TaskManagerAPI.Repositories
{
    public interface ITaskRepository
    {
      
            Task<List<TaskModel>> GetAllAsync();

            Task<TaskModel?> GetByIdAsync(Guid id);

            Task<TaskModel> CreateAsync(TaskModel taskModel);

            Task<TaskModel?> UpdateAsync(Guid id, TaskModel taskModel);

            Task<TaskModel?> UpdateStatusAsync(Guid id, int newStatusCategoryId);

            Task<TaskModel?> DeleteAsync(Guid id);

    }
}
