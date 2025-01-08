using Microsoft.AspNetCore.Mvc.Filters;
using TASK_FLOW.NET.Auth.Attributes;

namespace TASK_FLOW.NET.Auth.Helpers
{
    public static class AllowPublicAccessHelper
    {
        public static bool hasAllowPublicAccess(AuthorizationFilterContext context)
        {
            return context.ActionDescriptor.EndpointMetadata.Any(md => md is AllowAnonymousAccessAttribute);
        }
    }
}
