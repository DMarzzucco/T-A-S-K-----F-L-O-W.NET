using TASK_FLOW.NET.Auth.JWT.DTO;

namespace TASK_FLOW.NET.Auth.Cookie.Service.Interface
{
    public interface ICookieService
    {
        void SetTokenCookies(HttpResponse response, TokenPair tokens);
        void ClearTokenCookies(HttpResponse response);
    }
}
