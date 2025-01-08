using Microsoft.AspNetCore.Mvc.Filters;
using TASK_FLOW.NET.Auth.Attributes;
using TASK_FLOW.NET.Auth.Helpers;
using TASK_FLOW.NET.Auth.Service.Interface;
using TASK_FLOW.NET.User.Enums;

namespace TASK_FLOW.NET.Auth.Filters
{
    public class AccessLevelAuth : IAsyncAuthorizationFilter
    {
        private readonly IAuthService _authService;
        public AccessLevelAuth(IAuthService authService)
        {
            this._authService = authService;
        }

        public async Task OnAuthorizationAsync(AuthorizationFilterContext context)
        {
            if (AllowPublicAccessHelper.hasAllowPublicAccess(context)) return;

            var userRoleString = context.HttpContext.Items["UserRole"]?.ToString();

            if (!Enum.TryParse(userRoleString, out ROLES userRole))
            {
                context.Result = ContentResultHelper.CreateContentResult(StatusCodes.Status403Forbidden, "Invalid Rol");
                return;
            }
            if (userRole == ROLES.ADMIN || userRole == ROLES.CREATOR) return;

            var user = await this._authService.GetUserByCookie();
            var userExistInProject = user.ProjectIncludes.FirstOrDefault((pd) => pd.Project.Id == pd.ProjectId);

            if (userExistInProject == null)
            {
                context.Result = ContentResultHelper.CreateContentResult(StatusCodes.Status403Forbidden, "Don't belong to the project");
                return;
            }
            var level = userExistInProject.AccessLevel;

            var accessLevel = context.ActionDescriptor.EndpointMetadata.OfType<AccessLevelAttribute>().FirstOrDefault()?.AccessLevel;

            if (accessLevel == null || !accessLevel.Any(r => (int)level >= (int)r))
            {
                context.Result = ContentResultHelper.CreateContentResult(StatusCodes.Status403Forbidden, "Don't get the level access for this operation");
                return;
            }
        }
    }
}
