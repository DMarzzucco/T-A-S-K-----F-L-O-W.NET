using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Abstractions;
using Moq;
using TASK_FLOW.NET.Auth.Attributes;
using TASK_FLOW.NET.Auth.Filters;
using TASK_FLOW.NET.Auth.Service.Interface;
using TASK_FLOW.NET.User.Enums;
using TASK_FLOW.NET.UserProject.Enums;
using TASK_FLOW_TESTING.Auth.Helper;
using TASK_FLOW_TESTING.User.Mocks;

namespace TASK_FLOW_TESTING.Auth.Filters
{
    public class UnitAccessLevelAuthFilterTest
    {
        private readonly Mock<IAuthService> _authService;
        private readonly AccessLevelAuth _filter;

        public UnitAccessLevelAuthFilterTest()
        {
            this._authService = new Mock<IAuthService>();
            this._filter = new AccessLevelAuth(this._authService.Object);
        }

        // Return 403 when User don't belong to the project
        [Fact]
        public async Task ShouldReturn403WhenUserDontBelongToTheProject()
        {
            var context = MockCreateAuthorizationContext.CreateAuthorizationFilterContext();
            var actionDescriptor = new ActionDescriptor
            {
                EndpointMetadata = new List<object> {
                    new AccessLevelAttribute(ACCESSLEVEL.OWNER)
                }
            };
            context.ActionDescriptor = actionDescriptor;

            var user = UsersMock.UserMockWithOutProject;
            this._authService.Setup(s => s.GetUserByCookie()).ReturnsAsync(user);

            await this._filter.OnAuthorizationAsync(context);

            Assert.NotNull(context.Result);
            var result = Assert.IsType<ContentResult>(context.Result);
            Assert.Equal(StatusCodes.Status403Forbidden, result.StatusCode);
        }
        // Returns 403 when user Don't have the Access Level Required
        [Fact]
        public async Task ShouldReturn403WhenUserDontHaveTheAccessLevelRequired()
        {

            var context = MockCreateAuthorizationContext.CreateAuthorizationFilterContext();
            var actionDescriptor = new ActionDescriptor
            {
                EndpointMetadata = new List<object> {
                    new RolesAttribute(ROLES.BASIC),
                    new AccessLevelAttribute(ACCESSLEVEL.OWNER)
                }
            };
            context.ActionDescriptor = actionDescriptor;
            context.HttpContext.Items["UserRole"] = ROLES.BASIC.ToString();

            var user = UsersMock.UserMockWithRelations;
            this._authService.Setup(s => s.GetUserByCookie()).ReturnsAsync(user);

            await this._filter.OnAuthorizationAsync(context);

            Assert.NotNull(context.Result);
            var result = Assert.IsType<ContentResult>(context.Result);
            Assert.Equal(StatusCodes.Status403Forbidden, result.StatusCode);
        }
        // Allow User with Valid Access Level 
        [Fact]
        public async Task ShouldAllowUserWithValidAccessLevel() {
            var context = MockCreateAuthorizationContext.CreateAuthorizationFilterContext();
            var actionDescriptor = new ActionDescriptor
            {
                EndpointMetadata = new List<object> { 
                    new RolesAttribute(ROLES.BASIC),
                    new AccessLevelAttribute(ACCESSLEVEL.DEVELOPER)
                }
            };
            context.ActionDescriptor = actionDescriptor;
            context.HttpContext.Items["UserRole"] = ROLES.BASIC.ToString();

            var user = UsersMock.UserMockWithRelations;
            this._authService.Setup(s => s.GetUserByCookie()).ReturnsAsync(user);

            await this._filter.OnAuthorizationAsync(context);

            Assert.Null(context.Result);
        }
    }
}
