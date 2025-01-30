using TASK_FLOW.NET.Tasks.Model;

namespace TASK_FLOW.NET.Tasks.Repository.Interface
{
    public interface ITaskRepository
    {
        Task DeleteTaskAsync(TaskModel body);
        Task<TaskModel?> findByIdAsync(int id);
        Task<IEnumerable<TaskModel>> ListAllTaskAsync();
        Task<bool> SaveTaskAsync(TaskModel body);
        Task UpdateTaskAsync(TaskModel body);
    }
}
