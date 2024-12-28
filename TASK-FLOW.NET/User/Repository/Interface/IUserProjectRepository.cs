using TASK_FLOW.NET.User.Model;

namespace TASK_FLOW.NET.User.Repository.Interface
{
    public interface IUserProjectRepository
    {
        Task AddChangeAsync(UserProjectModel body);
        Task UpdateUPAsync(UserProjectModel body);
        Task <IEnumerable<UserProjectModel>>ListofAllAsync();
        Task<UserProjectModel?> findById(int id);
    }
}
