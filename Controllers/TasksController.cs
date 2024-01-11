using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Data;
using TaskManagerAPI.Models.Domain;
using TaskManagerAPI.Models.DTO;
using TaskManagerAPI.Repositories;

namespace TaskManagerAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class TasksController : ControllerBase
    {
        private readonly IMapper mapper;
        private readonly ITaskRepository taskRepository;
        private readonly IUserRepository userRepository;

        public TasksController(IMapper mapper, ITaskRepository taskRepository, IUserRepository userRepository)
        {
            this.mapper = mapper;
            this.taskRepository = taskRepository;
            this.userRepository = userRepository;
        }

        [HttpGet]
        [Authorize(Roles = "User,Admin")]
        public async Task<IActionResult> GetAll()
        {
            var tasks = await taskRepository.GetAllAsync();
            return Ok(mapper.Map<List<TaskDTO>>(tasks));

        }

        [HttpGet("{id}")]
        [Authorize(Roles = "User,Admin")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var task = await taskRepository.GetByIdAsync(id);
            if (task == null) return NotFound();

            return Ok(mapper.Map<TaskDTO>(task));

        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Create([FromBody] AddTaskDTO addTaskDTO)
        {
            var taskDomainModel = mapper.Map<TaskModel>(addTaskDTO);

            // Set the default value here
            if (taskDomainModel.StatusCategoryId == 0)
            {
                taskDomainModel.StatusCategoryId = 1;
            }

            var assignedUser = await userRepository.GetByIdAsync(taskDomainModel.AssignedUserId);
            if (assignedUser != null)
            {
                Console.WriteLine($"Assigned User ID: {assignedUser.UserId}");
                Console.WriteLine($"Assigned User Name: {assignedUser.UserName}");

                // Set the UserName property before saving to the database
                taskDomainModel.UserName = assignedUser.UserName;

            }
            else
            {
                Console.WriteLine("Assigned user is null!");
                return BadRequest("Assigned user not found.");
            }

            taskDomainModel = await taskRepository.CreateAsync(taskDomainModel);

            // Populate the DTO
            var createdTaskDTO = mapper.Map<TaskDTO>(taskDomainModel);

            return CreatedAtAction(nameof(GetById), new { id = createdTaskDTO.TaskId }, createdTaskDTO);
        }


        [HttpPut("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Update(Guid id, [FromBody] UpdateTaskDTO updateTaskDTO)
        {
            var existingTask = await taskRepository.GetByIdAsync(id);
            if (existingTask == null)
            {
                return NotFound();
            }

            mapper.Map(updateTaskDTO, existingTask);

            var updatedTask = await taskRepository.UpdateAsync(id, existingTask);

            if (updatedTask == null)
            {
                return NotFound();
            }

            return Ok(mapper.Map<TaskDTO>(updatedTask));
        }


        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var deletedTask = await taskRepository.DeleteAsync(id);
            if (deletedTask == null) return NotFound();

            return Ok(mapper.Map<TaskDTO>(deletedTask));
        }


        [HttpPut("{id}/status/{statusCategoryId}")]
        [Authorize(Roles = "User, Admin")]
        public async Task<IActionResult> UpdateStatus(Guid id, int statusCategoryId)
        {
            var updatedTask = await taskRepository.UpdateStatusAsync(id, statusCategoryId);
            if (updatedTask == null) return NotFound();

            return Ok(mapper.Map<TaskDTO>(updatedTask));

        }
    }
}
