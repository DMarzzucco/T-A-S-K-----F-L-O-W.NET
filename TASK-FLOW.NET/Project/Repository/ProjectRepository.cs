using Microsoft.EntityFrameworkCore;
using TASK_FLOW.NET.Context;
using TASK_FLOW.NET.Project.Model;
using TASK_FLOW.NET.Project.Repository.Interface;
using TASK_FLOW.NET.Tasks.Model;
using TASK_FLOW.NET.UserProject.Model;

namespace TASK_FLOW.NET.Project.Repository
{
    public class ProjectRepository : IProjectRepository
    {
        private readonly AppDBContext _context;
        public ProjectRepository(AppDBContext context)
        {
            this._context = context;
        }

        public async Task DeleteProjectAsync(ProjectModel body)
        {
            this._context.ProjectModel.Remove(body);
            await this._context.SaveChangesAsync();
        }

        public async Task<ProjectModel?> findByIdAsync(int id)
        {
            var project = await this._context.ProjectModel
                .Include(u => u.UsersIncludes)
                .ThenInclude(ui => ui.User)
                .FirstOrDefaultAsync(p => p.Id == id);
            if (project != null && project.UsersIncludes != null && project.UsersIncludes.Any())
                return project;

            project.UsersIncludes = new List<UserProjectModel>();
            return project;
        }

        public async Task<IEnumerable<ProjectModel>> ListOfProjectAsync()
        {
            return await this._context.ProjectModel.Select(u => new ProjectModel
            {
                Id = u.Id,
                Name = u.Name,
                Description = u.Description,
                Tasks = new List<TaskModel>(),
                UsersIncludes = new List<UserProjectModel>()

            }).ToListAsync();
        }

        public async Task SaveProjectAsync(ProjectModel body)
        {
            this._context.ProjectModel.Add(body);
            await this._context.SaveChangesAsync();
        }

        public async Task UpdateProjectAsync(ProjectModel body)
        {
            this._context.ProjectModel.Entry(body).State = EntityState.Modified;
            await this._context.SaveChangesAsync();
        }
    }
}
