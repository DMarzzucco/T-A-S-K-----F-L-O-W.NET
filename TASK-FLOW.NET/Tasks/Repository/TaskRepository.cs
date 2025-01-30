using Microsoft.EntityFrameworkCore;
using TASK_FLOW.NET.Context;
using TASK_FLOW.NET.Tasks.Model;
using TASK_FLOW.NET.Tasks.Repository.Interface;

namespace TASK_FLOW.NET.Tasks.Repository
{
    public class TaskRepository : ITaskRepository
    {
        private readonly AppDBContext _context;
        public TaskRepository(AppDBContext context)
        {
            this._context = context;
        }

        public async Task DeleteTaskAsync(TaskModel body)
        {
            this._context.TaskModel.Remove(body);
            await this._context.SaveChangesAsync();
        }

        public async Task<TaskModel?> findByIdAsync(int id)
        {
            var tasks = await this._context.TaskModel
                .Where(t => t.Id == id).Select(t => new TaskModel
                {
                    Id = t.Id,
                    Name = t.Name,
                    Descritpion = t.Descritpion,
                    Status = t.Status,
                    ResponsibleName = t.ResponsibleName,
                    ProjectId = t.ProjectId,
                    Project = t.Project
                })
                .FirstOrDefaultAsync();
            return tasks;
        }

        public async Task<IEnumerable<TaskModel>> ListAllTaskAsync()
        {
            return await this._context.TaskModel.Select(t => new TaskModel
            {
                Id = t.Id,
                Name = t.Name,
                Descritpion = t.Descritpion,
                Status = t.Status,
                ResponsibleName = t.ResponsibleName,
                ProjectId = t.ProjectId,
                Project = null
            }).ToListAsync();
        }

        public async Task<bool> SaveTaskAsync(TaskModel body)
        {
            var project = await this._context.ProjectModel.FindAsync(body.ProjectId);

            if (project == null) return false;

            this._context.Attach(project);

            this._context.TaskModel.Add(body);
            await this._context.SaveChangesAsync();
            return true;
        }

        public async Task UpdateTaskAsync(TaskModel body)
        {
            this._context.TaskModel.Entry(body).State = EntityState.Modified;
            await this._context.SaveChangesAsync();
        }
    }
}
