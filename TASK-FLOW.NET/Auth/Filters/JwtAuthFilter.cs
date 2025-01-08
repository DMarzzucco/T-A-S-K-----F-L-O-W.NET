using Microsoft.AspNetCore.Mvc.Filters;
using TASK_FLOW.NET.Auth.Helpers;
using TASK_FLOW.NET.Auth.JWT.Service.Interface;
using TASK_FLOW.NET.Auth.Service.Interface;

namespace TASK_FLOW.NET.Auth.Filters
{
    public class JwtAuthFilter : IAsyncAuthorizationFilter
    {
        private readonly IAuthService _authService;
        private readonly ITokenService _tokenService;

        public JwtAuthFilter(IAuthService authService, ITokenService tokenService)
        {
            this._authService = authService;
            this._tokenService = tokenService;
        }

        public async Task OnAuthorizationAsync(AuthorizationFilterContext context)
        {
            if (AllowPublicAccessHelper.hasAllowPublicAccess(context)) return;

            var token = context.HttpContext.Request.Cookies["Authentication"];
            if (string.IsNullOrEmpty(token))
            {
                context.Result = ContentResultHelper.CreateContentResult(StatusCodes.Status401Unauthorized, "Invalid or not Provider Token");
                return;
            }
            this._tokenService.ValidateToken(token);

            var user = await this._authService.GetUserByCookie();

            context.HttpContext.Items["UserId"] = user.Id;
            context.HttpContext.Items["UserRole"] = user.Roles;
        }
    }
}
