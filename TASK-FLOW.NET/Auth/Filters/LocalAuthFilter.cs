using Microsoft.AspNetCore.Mvc.Filters;
using TASK_FLOW.NET.Auth.DTO;
using TASK_FLOW.NET.Auth.Service.Interface;

namespace TASK_FLOW.NET.Auth.Filters
{
    public class LocalAuthFilter : IAsyncActionFilter
    {
        private readonly IAuthService _authService;

        public LocalAuthFilter(IAuthService authService)
        {
            this._authService = authService;
        }

        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var body = context.ActionArguments["body"] as AuthPropsDTO;

            var user = await this._authService.ValidationUser(body);
            context.HttpContext.Items["User"] = user;

            await next();
        }
    }
}
