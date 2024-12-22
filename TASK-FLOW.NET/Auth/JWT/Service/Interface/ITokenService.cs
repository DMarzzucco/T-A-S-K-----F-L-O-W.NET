using TASK_FLOW.NET.Auth.JWT.DTO;
using TASK_FLOW.NET.User.Model;

namespace TASK_FLOW.NET.Auth.JWT.Service.Interface
{
    public interface ITokenService
    {
        TokenPair GenerateToken(UsersModel user);
        int ValidateToken(string token, string claimtype);
        bool ValidateRefreshToken(string refreshToken, string storedRefreshToken);
    }
}
