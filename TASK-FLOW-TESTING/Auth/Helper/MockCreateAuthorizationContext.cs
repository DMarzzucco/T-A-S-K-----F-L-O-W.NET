
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;

namespace TASK_FLOW_TESTING.Auth.Helper
{
    public class MockCreateAuthorizationContext
    {
        public static AuthorizationFilterContext CreateAuthorizationFilterContext()
        {
            var context = new DefaultHttpContext();
            var actionContext = new ActionContext
            {
                HttpContext = context,
                RouteData = new Microsoft.AspNetCore.Routing.RouteData(),
                ActionDescriptor = new Microsoft.AspNetCore.Mvc.Abstractions.ActionDescriptor()
            };
            var filterContext = new AuthorizationFilterContext(actionContext, new List<IFilterMetadata>());

            return filterContext;
        }
    }
}
