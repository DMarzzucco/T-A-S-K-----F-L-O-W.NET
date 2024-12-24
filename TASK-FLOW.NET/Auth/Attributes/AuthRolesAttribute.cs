using Microsoft.AspNetCore.Mvc;
using TASK_FLOW.NET.Auth.Filters;

namespace TASK_FLOW.NET.Auth.Attributes
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public class AuthRolesAttribute:TypeFilterAttribute
    {
        public AuthRolesAttribute():base (typeof(RolesValidationFilters))
        {
            
        }
    }
}
