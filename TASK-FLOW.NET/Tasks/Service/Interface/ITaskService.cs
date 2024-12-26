using TASK_FLOW.NET.Tasks.DTOs;
using TASK_FLOW.NET.Tasks.Model;

namespace TASK_FLOW.NET.Tasks.Service.Interface
{
    public interface ITaskService
    {
        Task<TaskModel> CreateTask(int ProjectId, CreateTaskDTO body);
        Task DeleteTask(int id);
        Task<IEnumerable<TaskModel>> ListOfAllTask();
        Task<TaskModel> GetTaskById(int id);
        Task<TaskModel> UpdateTask(int id, UpdateTaskDTO body);

    }
}
