using TASK_FLOW.NET.Auth.JWT.DTO;

namespace TASK_FLOW.NET.Auth.Cookie.Service.Interface
{
    public interface ICookieService
    {
        void SetTokenCookies(HttpResponse response, TokenPair tokens);
        string GetCookies(HttpRequest request, string cookieName);
        void ClearTokenCookies(HttpResponse response);
    }
}
