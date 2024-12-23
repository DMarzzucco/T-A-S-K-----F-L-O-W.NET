using Microsoft.AspNetCore.Mvc;
using TASK_FLOW.NET.Auth.Filters;

namespace TASK_FLOW.NET.Auth.Attributes
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public class JwtAuthAttribute:TypeFilterAttribute
    {
        public JwtAuthAttribute(): base (typeof(JwtAuthFilter))
        {
            
        }
    }
}
