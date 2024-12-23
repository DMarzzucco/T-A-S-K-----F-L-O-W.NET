using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using TASK_FLOW.NET.Auth.Attributes;
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
            var hasAllowAnonymousAccess = context.ActionDescriptor.EndpointMetadata.Any(md => md is AllowAnonymousAccessAttribute);
            if (hasAllowAnonymousAccess) return;

            var token = context.HttpContext.Request.Cookies["Authentication"];
            if (string.IsNullOrEmpty(token))
            {
                context.Result = new ContentResult
                {
                    StatusCode = StatusCodes.Status401Unauthorized,
                    Content = "Invalid or not Provider token.",
                    ContentType = "application/json"
                };
                return;
            }
            this._tokenService.ValidateToken(token);

            var user = await this._authService.GetUserByCookie();

            context.HttpContext.Items["UserId"] = user.Id;
            context.HttpContext.Items["UserRole"] = user.Roles;
        }
    }
}
