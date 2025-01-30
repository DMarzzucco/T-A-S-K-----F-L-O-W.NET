using TASK_FLOW.NET.UserProject.Model;

namespace TASK_FLOW.NET.UserProject.Repository.Interface
{
    public interface IUserProjectRepository
    {
        Task<bool> AddChangeAsync(UserProjectModel body);
        Task<bool> UpdateUPAsync(UserProjectModel body);
        Task<IEnumerable<UserProjectModel>> ListofAllAsync();
        Task<UserProjectModel?> findById(int id);
    }
}
