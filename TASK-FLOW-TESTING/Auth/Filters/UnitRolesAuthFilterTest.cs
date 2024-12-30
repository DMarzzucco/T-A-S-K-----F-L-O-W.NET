using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using TASK_FLOW.NET.Auth.Filters;
using TASK_FLOW.NET.Auth.Attributes;
using Microsoft.AspNetCore.Mvc.Abstractions;
using TASK_FLOW.NET.User.Enums;
using TASK_FLOW_TESTING.Auth.Helper;

namespace TASK_FLOW_TESTING.Auth.Filters
{
    public class UnitRolesAuthFilterTest
    {
        //Return a 403 when User Role is Empty
        [Fact]
        public void ShouldReturn403WhenUserRoleIsEmpty()
        {
            var context = MockCreateAuthorizationContext.CreateAuthorizationFilterContext();

            var actionDescriptor = new ActionDescriptor
            {
                EndpointMetadata = new List<object> {
                      new RolesAttribute(ROLES.ADMIN)
                }
            };
            context.ActionDescriptor = actionDescriptor;

            context.HttpContext.Items["UserRole"] = null;

            var filter = new RolesValidationFilters();
            filter.OnAuthorization(context);

            Assert.NotNull(context.Result);
            var result = Assert.IsType<ContentResult>(context.Result);
            Assert.Equal(StatusCodes.Status403Forbidden, result.StatusCode);
        }
        //Return 403 When User Role Does Not Have Access
        [Fact]
        public void ShouldReturn403WhenUserRoleDoesNotHaveAccess()
        {
            var context = MockCreateAuthorizationContext.CreateAuthorizationFilterContext();
            var actionDescriptor = new ActionDescriptor
            {
                EndpointMetadata = new List<object> {
                    new RolesAttribute (ROLES.ADMIN)
                }
            };
            context.ActionDescriptor = actionDescriptor;
            context.HttpContext.Items["UserRole"] = ROLES.BASIC.ToString();

            var filter = new RolesValidationFilters();
            filter.OnAuthorization(context);

            Assert.NotNull(context.Result);
            var result = Assert.IsType<ContentResult>(context.Result);
            Assert.Equal(StatusCodes.Status403Forbidden, result.StatusCode);
        }

        // Allow Access When User Roles Is Valid
        [Fact]
        public void ShouldAllowAccessWhenUserRoleIsValid() {
            var context = MockCreateAuthorizationContext.CreateAuthorizationFilterContext();
            var actionDescriptor = new ActionDescriptor
            {
                EndpointMetadata = new List<object> {
                    new RolesAttribute (ROLES.CREATOR)
                }
            };
            context.ActionDescriptor = actionDescriptor;
            context.HttpContext.Items["UserRole"] = ROLES.CREATOR.ToString();

            var filter = new RolesValidationFilters();
            filter.OnAuthorization(context);
            Assert.Null(context.Result);
        }
    }
}
