using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using TASK_FLOW.NET.Auth.Attributes;
using TASK_FLOW.NET.User.Enums;

namespace TASK_FLOW.NET.Auth.Filters
{
    public class RolesValidationFilters : IAuthorizationFilter
    {

        public void OnAuthorization(AuthorizationFilterContext context)
        {
            var hasAllowPublicAccess = context.ActionDescriptor.EndpointMetadata.Any(md => md is AllowAnonymousAccessAttribute);
            if (hasAllowPublicAccess) return;

            var userRoleString = context.HttpContext.Items["UserRole"]?.ToString();
            if (string.IsNullOrEmpty(userRoleString))
            {
                context.Result = new ContentResult
                {
                    StatusCode = StatusCodes.Status403Forbidden,
                    Content = "Empty Rol",
                    ContentType = "application/json"
                };
            }
            if (!Enum.TryParse(userRoleString, out ROLES userRole))
            {
                context.Result = new ContentResult
                {
                    StatusCode = StatusCodes.Status403Forbidden,
                    Content = "Invalid Rol",
                    ContentType = "application/json"
                };
            }
            if (userRole == ROLES.ADMIN) return;

            var requiredRol = context.ActionDescriptor.EndpointMetadata.OfType<RolesAttribute>().FirstOrDefault()?.Roles;

            bool isAuth = requiredRol.Any(r => (int)userRole <= (int)r);
            if (!isAuth)
            {
                context.Result = new ContentResult
                {
                    StatusCode = StatusCodes.Status403Forbidden,
                    Content = "You are not access",
                    ContentType = "application/json"
                };
            }
        }
    }
}
