using Microsoft.IdentityModel.Tokens;
using TASK_FLOW.NET.Auth.Cookie.Service.Interface;
using TASK_FLOW.NET.Auth.JWT.Service.Interface;
using TASK_FLOW.NET.Auth.Service.Interface;

namespace TASK_FLOW.NET.Auth.Middleware
{
    public class RefreshTokenMiddleware
    {
        private readonly RequestDelegate _next;

        public RefreshTokenMiddleware(RequestDelegate next)
        {
            this._next = next;
        }
        public async Task InvokeAsync(HttpContext context, ITokenService tokenService, IAuthService authService, ICookieService cookieService)
        {
            var publicPaths = new[] { "/api/Auth/login", "/api/User/register" };

            var path = context.Request.Path.Value;
            if (publicPaths.Contains(path))
            {
                await _next(context);
                return;
            }

            var accessToken = context.Request.Cookies["Authentication"];

            if (string.IsNullOrEmpty(accessToken))
            {
                context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                await context.Response.WriteAsJsonAsync(new { message = "Token missing" });
                return;
            }
            try
            {
                if (!tokenService.ValidateToken(accessToken))
                {
                    context.Response.StatusCode = StatusCodes.Status403Forbidden;
                    await context.Response.WriteAsJsonAsync(new { message = "Invalid Token" });
                    return;
                }
                if (tokenService.isTokenExpireSoon(accessToken))
                {
                    var user = await authService.GetUserByCookie();

                    var refreshToken = context.Request.Cookies["RefreshToken"];
                    if (string.IsNullOrEmpty(refreshToken))
                    {
                        context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                        await context.Response.WriteAsJsonAsync(new { message = "Refresh Token is missing " });
                        return;
                    }
                    if (!tokenService.ValidateToken(refreshToken))
                    {
                        context.Response.StatusCode = StatusCodes.Status403Forbidden;
                        await context.Response.WriteAsJsonAsync(new { message = "Invalid refresh Token" });
                        return;
                    }
                    var newAccessToken = tokenService.RefreshToken(user);
                    cookieService.SetTokenCookies(context.Response, newAccessToken);
                }
            }
            catch (SecurityTokenExpiredException ex)
            {
                context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                await context.Response.WriteAsJsonAsync(new { message = ex.Message });
                return;
            }

            await this._next(context);
        }
    }
}
