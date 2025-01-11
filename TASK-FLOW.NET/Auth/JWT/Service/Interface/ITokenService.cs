using TASK_FLOW.NET.Auth.JWT.DTO;
using TASK_FLOW.NET.User.Model;

namespace TASK_FLOW.NET.Auth.JWT.Service.Interface
{
    public interface ITokenService
    {
        TokenPair GenerateToken(UsersModel user);
        TokenPair RefreshToken(UsersModel user);
        bool ValidateToken(string token);
        int GetIdFromToken();
        bool isTokenExpireSoon(string token);
        TokenPair CreateTokenPair(UsersModel user, DateTime accessTokenExpired, DateTime refreshTokenExpired);
    }
}
