using TaskManagerAPI.Data;
using TaskManagerAPI.Models.Domain;
using Microsoft.EntityFrameworkCore;


namespace TaskManagerAPI.Repositories
{
    public class SQLProjectRepository : IProjectRepository
    {
        private readonly TaskDbContext dbContext;

        public SQLProjectRepository(TaskDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task<Project> CreateAsync(Project project )
        {
            await dbContext.Projects.AddAsync(project);
            await dbContext.SaveChangesAsync();
            return project;
        }

        public async Task<Project?> DeleteAsync(Guid id)
        {
            var existingProject = await dbContext.Projects.FirstOrDefaultAsync(x => x.ProjectId == id);

            if (existingProject == null)
            {
                return null;
            }

            dbContext.Projects.Remove(existingProject);
            await dbContext.SaveChangesAsync();
            return existingProject;
        }

        public async Task<List<Project>> GetAllAsync()
        {
            return await dbContext.Projects.ToListAsync();
        }

        public async Task<Project?> GetByIdAsync(Guid id)
        {
            return await dbContext.Projects
                .Include(p => p.Tasks)
                    .ThenInclude(t => t.StatusCategory)
                .FirstOrDefaultAsync(x => x.ProjectId == id);
        }

        public async Task<Project?> UpdateAsync(Guid id, Project project)
        {
            var existingProject = await dbContext.Projects.FirstOrDefaultAsync(x => x.ProjectId == id);

            if (existingProject == null)
            {
                return null;
            }

            existingProject.Title = project.Title;

            await dbContext.SaveChangesAsync();
            return existingProject;
        }
    }
}
