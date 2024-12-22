using TASK_FLOW.NET.Auth.Cookie.Service.Interface;
using TASK_FLOW.NET.Auth.JWT.DTO;

namespace TASK_FLOW.NET.Auth.Cookie.Service
{
  
    public class CookieService : ICookieService
    {
        public void ClearTokenCookies(HttpResponse response)
        {
            response.Cookies.Append("Authentication", "", new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                Expires = DateTime.UnixEpoch,
                SameSite = SameSiteMode.Strict
            });

            response.Cookies.Append("RefreshToken", "", new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                Expires = DateTime.UnixEpoch,
                SameSite = SameSiteMode.Strict
            });
        }

        public string GetCookies(HttpRequest request, string cookieName)
        {
            if (request.Cookies.TryGetValue(cookieName, out var cookieValue)) return cookieValue;
            return null;
        }

        public void SetTokenCookies(HttpResponse response, TokenPair tokens)
        {
            response.Cookies.Append("Authentication", tokens.AccessToken, new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                Expires = DateTime.UtcNow.AddDays(2),
                SameSite = SameSiteMode.Strict
            });
            response.Cookies.Append("RefreshToken", tokens.RefreshToken, new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                Expires = DateTime.UtcNow.AddDays(5),
                SameSite = SameSiteMode.Strict
            });
        }
    }
}
