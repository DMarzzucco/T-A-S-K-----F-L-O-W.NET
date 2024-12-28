using TASK_FLOW.NET.Project.DTO;
using TASK_FLOW.NET.Project.Model;
using TASK_FLOW.NET.UserProject.Model;

namespace TASK_FLOW.NET.Project.Service.Interface
{
    public interface IProjectService
    {
        Task<UserProjectModel> SaveProject(CreateProjectDTO body);
        Task<IEnumerable<ProjectModel>> ListOfProject();
        Task<ProjectModel> GetProjectById(int id);
        Task<ProjectModel> UpdateProject(int id, UpdateProjectDTO body);
        Task DeleteProject(int id);
    }
}
