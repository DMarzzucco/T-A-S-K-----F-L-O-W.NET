using Microsoft.EntityFrameworkCore;
using TASK_FLOW.NET.Context;
using TASK_FLOW.NET.Project.Model;
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
                    Project = null
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

        public async Task SaveTaskAsync(TaskModel body)
        {
            this._context.TaskModel.Add(body);
            await this._context.SaveChangesAsync();
        }

        public async Task UpdateTaskAsync(TaskModel body)
        {
            this._context.TaskModel.Entry(body).State = EntityState.Modified;
            await this._context.SaveChangesAsync();
        }
    }
}
