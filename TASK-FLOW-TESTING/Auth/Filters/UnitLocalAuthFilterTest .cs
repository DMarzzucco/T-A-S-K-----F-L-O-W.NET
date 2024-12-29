using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Moq;
using TASK_FLOW.NET.Auth.DTO;
using TASK_FLOW.NET.Auth.Filters;
using TASK_FLOW.NET.Auth.Service.Interface;
using TASK_FLOW_TESTING.Auth.Mock;
using TASK_FLOW_TESTING.User.Mocks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace TASK_FLOW_TESTING.Auth.Filters
{
    public class UnitLocalAuthAuthFilterTest
    {
        [Fact]
        public async Task OnActionExcutionAsync_ShouldValidateTheUserCredentials()
        {
            var mockAuthService = new Mock<IAuthService>();
            var authPropsDTO = AuthMock.AuthDTOMock;
            var user = UsersMock.UserHashPassMock;

            mockAuthService.Setup(s => s.ValidationUser(It.IsAny<AuthPropsDTO>())).ReturnsAsync(user);

            var filter = new LocalAuthFilter(mockAuthService.Object);
            var httpContext = new DefaultHttpContext();
            var actionContext = new ActionContext(httpContext, new RouteData(), new ActionDescriptor(), new ModelStateDictionary()  );


            var context = new ActionExecutingContext(
                actionContext,
                new List<IFilterMetadata>(),
                new Dictionary<string, object> { { "body", authPropsDTO} },
                new object()
                );
            var exceuteContext = new ActionExecutedContext(
                actionContext,
                new List<IFilterMetadata>(),
                context.Controller
                );

            await filter.OnActionExecutionAsync(context, () => Task.FromResult(exceuteContext));

            Assert.Equal(user, context.HttpContext.Items["User"]);
        }
    }
}
