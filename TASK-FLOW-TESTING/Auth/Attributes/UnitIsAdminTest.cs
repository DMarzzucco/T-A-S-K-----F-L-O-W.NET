
using Microsoft.AspNetCore.Mvc.Abstractions;
using Moq;
using TASK_FLOW.NET.Auth.Attributes;
using TASK_FLOW.NET.Auth.Filters;
using TASK_FLOW.NET.Auth.Service.Interface;
using TASK_FLOW.NET.User.Enums;
using TASK_FLOW_TESTING.Auth.Helper;

namespace TASK_FLOW_TESTING.Auth.Attributes
{
    public class UnitIsAdminTest
    {
        private readonly Mock<IAuthService> _authSerivce;
        private readonly AccessLevelAuth _accessLevelFilter;

        public UnitIsAdminTest()
        {
            this._authSerivce = new Mock<IAuthService>();
            this._accessLevelFilter = new AccessLevelAuth(this._authSerivce.Object);
        }

        [Fact]
        public async Task ShouldGetAccessWhenTheUserRolesIsAdmin() {
            var context = MockCreateAuthorizationContext.CreateAuthorizationFilterContext();
            var actionDescriptor = new ActionDescriptor { 
                EndpointMetadata = new List<object> { 
                    new RolesAttribute(ROLES.ADMIN)
                }
            };
            context.ActionDescriptor = actionDescriptor;
            context.HttpContext.Items["UserRole"] = ROLES.ADMIN.ToString();

            var Rolesfilter = new RolesValidationFilters();
            Rolesfilter.OnAuthorization(context);
            await this._accessLevelFilter.OnAuthorizationAsync(context);

            Assert.Null(context.Result);
        }
    }
}
