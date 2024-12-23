using TASK_FLOW.NET.Auth.JWT.DTO;
using TASK_FLOW.NET.User.Model;

namespace TASK_FLOW.NET.Auth.JWT.Service.Interface
{
    public interface ITokenService
    {
        TokenPair GenerateToken(UsersModel user);
        void ValidateToken(string token);
        int GetIdFromToken();
    }
}
