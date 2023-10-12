using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using TaskManagerAPI.Models.Domain;
using TaskManagerAPI.Models.DTO;
using TaskManagerAPI.Repositories;

namespace TaskManagerAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ProjectsController : ControllerBase

    {
        private readonly IMapper mapper;
        private readonly IProjectRepository projectRepository;

        public ProjectsController(IMapper mapper, IProjectRepository projectRepository)
        {
            this.mapper = mapper;
            this.projectRepository = projectRepository;
        }

        [HttpGet]
        [Authorize(Roles = "User, Admin")]
        public async Task<IActionResult> GetAll()
        {
            var projectsDomain = await projectRepository.GetAllAsync();
            return Ok(mapper.Map<List<ProjectDTO>>(projectsDomain));
        }


        [HttpGet("{id:Guid}")]
        [Authorize(Roles = "User, Admin")]
        public async Task<IActionResult> GetById([FromRoute] Guid id)
        {
            var projectDomain = await projectRepository.GetByIdAsync(id);

            if (projectDomain == null)
            {
                return NotFound();
            }

            return Ok(mapper.Map<ProjectDTO>(projectDomain));
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Create([FromBody] AddProjectDTO addProjectDTO)


        {   //Map Dto to Domain model
            var ProjectDomainModel = mapper.Map<Project>(addProjectDTO);

            ProjectDomainModel = await projectRepository.CreateAsync(ProjectDomainModel);

            var projectDto = mapper.Map<ProjectDTO>(ProjectDomainModel);


            return CreatedAtAction(nameof(GetById), new { id = projectDto.ProjectId }, projectDto);
        }


        [HttpPut]
        [Route("{id:Guid}")]
        [Authorize(Roles = "Admin")]

        public async Task<IActionResult> Update([FromRoute] Guid id, [FromBody] UpdateProjectDTO updateProjectDTO)
        {


            var projectDomainModel = mapper.Map<Project>(updateProjectDTO);

            // Check if region exists
            projectDomainModel = await projectRepository.UpdateAsync(id, projectDomainModel);

            if (projectDomainModel == null)
            {
                return NotFound();
            }

            return Ok(mapper.Map<ProjectDTO>(projectDomainModel));
        }



        [HttpDelete("{id:Guid}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteProject(Guid id)
        {
            var deletedProject = await projectRepository.DeleteAsync(id);

            if (deletedProject == null)
            {
                return NotFound();
            }

            return NoContent();
        }




    }
}
