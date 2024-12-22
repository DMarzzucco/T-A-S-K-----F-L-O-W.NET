using TASK_FLOW.NET.Auth.DTO;
using TASK_FLOW.NET.User.Model;

namespace TASK_FLOW.NET.Auth.Service.Interface
{
    public interface IAuthService
    {
        Task<UsersModel> ValidationUser(AuthPropsDTO body);
        Task<string> GenerateToken(UsersModel body);
        Task<UsersModel> GetUserByCookie();
        Task<string> GetProfile();
        Task<UsersModel> RefreshTokenValidate(string refreshToken, int id);
        Task Logout();

    }
}
