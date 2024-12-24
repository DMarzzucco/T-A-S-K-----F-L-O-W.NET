using TASK_FLOW.NET.Project.Model;

namespace TASK_FLOW.NET.Project.Repository.Interface
{
    public interface IProjectRepository
    {
        Task SaveProjectAsync(ProjectModel body);
        Task<IEnumerable<ProjectModel>> ListOfProjectAsync();
        Task<ProjectModel?> findByIdAsync(int id);
        Task UpdateProjectAsync(ProjectModel body);
        Task DeleteProjectAsync(ProjectModel body);
    }
}
