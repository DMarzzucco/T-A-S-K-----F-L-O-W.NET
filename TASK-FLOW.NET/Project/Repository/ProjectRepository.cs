using Microsoft.EntityFrameworkCore;
using TASK_FLOW.NET.Context;
using TASK_FLOW.NET.Project.Model;
using TASK_FLOW.NET.Project.Repository.Interface;

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
            return await this._context.ProjectModel.FindAsync(id);
        }

        public async Task<IEnumerable<ProjectModel>> ListOfProjectAsync()
        {
            return await this._context.ProjectModel.ToListAsync();
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
