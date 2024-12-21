using TASK_FLOW.NET.User.DTO;
using TASK_FLOW.NET.User.Model;

namespace TASK_FLOW.NET.User.Service.Interface
{
    public interface IUserService
    {
        Task<IEnumerable<UsersModel>> GetAll();
        Task<UsersModel> GetById(int id);
        Task<UsersModel> CreateUser(CreateUserDTO body);
        Task<UsersModel> UpdateUser(int id, UpdateUserDTO body);
        Task DeleteUser(int id);
        Task<UsersModel> FindByAuth(string key, object value);
        Task<UsersModel> UpdateToken(int id, string RefreshToken);
        Task<UserProjectModel> RelationProject(UserProjectDTO body);
    }
}
