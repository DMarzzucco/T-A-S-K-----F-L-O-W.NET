using TASK_FLOW.NET.User.DTO;
using TASK_FLOW.NET.User.Model;

namespace TASK_FLOW.NET.User.Service.Interface
{
    public interface IUserProjectService
    {
        Task<UserProjectModel> UpdateUP(int id, UpdateUserProjectDTO body);
        Task<IEnumerable<UserProjectModel>> ListOfAllUP();
        Task<UserProjectModel> GetUPbyID(int id);
        Task<UserProjectModel> CreateUP(UserProjectDTO body);
    }
}
