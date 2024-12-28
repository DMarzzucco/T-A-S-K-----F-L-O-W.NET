using TASK_FLOW.NET.UserProject.DTO;
using TASK_FLOW.NET.UserProject.Model;

namespace TASK_FLOW.NET.UserProject.Services.Interface
{
    public interface IUserProjectService
    {
        Task<UserProjectModel> UpdateUP(int id, UpdateUserProjectDTO body);
        Task<IEnumerable<UserProjectModel>> ListOfAllUP();
        Task<UserProjectModel> GetUPbyID(int id);
        Task<UserProjectModel> CreateUP(UserProjectDTO body);
    }
}
