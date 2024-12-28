using TASK_FLOW.NET.UserProject.Model;

namespace TASK_FLOW.NET.UserProject.Repository.Interface
{
    public interface IUserProjectRepository
    {
        Task AddChangeAsync(UserProjectModel body);
        Task UpdateUPAsync(UserProjectModel body);
        Task<IEnumerable<UserProjectModel>> ListofAllAsync();
        Task<UserProjectModel?> findById(int id);
    }
}
