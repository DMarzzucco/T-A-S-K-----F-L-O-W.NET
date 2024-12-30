using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using TASK_FLOW.NET.Auth.Filters;
using TASK_FLOW.NET.Auth.JWT.Service.Interface;
using TASK_FLOW.NET.Auth.Service.Interface;
using TASK_FLOW_TESTING.Auth.Mock;
using TASK_FLOW_TESTING.User.Mocks;
using TASK_FLOW_TESTING.Auth.Helper;

namespace TASK_FLOW_TESTING.Auth.Filters
{
    public class UnitJWTAuthFilterTest
    {
        private readonly Mock<IAuthService> _authService;
        private readonly Mock<ITokenService> _tokenService;
        private readonly JwtAuthFilter _filter;

        public UnitJWTAuthFilterTest()
        {
            this._authService = new Mock<IAuthService>();
            this._tokenService = new Mock<ITokenService>();
            this._filter = new JwtAuthFilter(this._authService.Object, this._tokenService.Object);
        }
        //Should Set user context when token is valid
        [Fact]
        public async Task OnAuthorizationShouldSetUserContextWhenTokenIsValid()
        {
            var context = MockCreateAuthorizationContext.CreateAuthorizationFilterContext();
            var validToken = AuthMock.TokenMock.AccessToken;
            var user = UsersMock.UserMock;


            var cookie = new Mock<IRequestCookieCollection>();

            cookie.Setup(c => c["Authentication"]).Returns(validToken);

            var httpContext = new Mock<HttpContext>();
            var items = new Dictionary<object, object>();
            httpContext.Setup(h => h.Request.Cookies).Returns(cookie.Object);
            httpContext.Setup(h => h.Items).Returns(items);

            context.HttpContext = httpContext.Object;

            this._tokenService.Setup(t => t.ValidateToken(validToken)).Verifiable();
            this._authService.Setup(a => a.GetUserByCookie()).ReturnsAsync(user);

            await this._filter.OnAuthorizationAsync(context);

            //Assert.NotNull(context.Result);
            Assert.Equal(user.Id, context.HttpContext.Items["UserId"]);
            Assert.Equal(user.Roles, context.HttpContext.Items["UserRole"]);
        }

        //Should Set Unauthorized When Token Is Null Or Empty
        [Fact]
        public async Task ShouldSetUnauthorizedWhenTokenIsNullOrEmpty()
        {
            var context = MockCreateAuthorizationContext.CreateAuthorizationFilterContext();
            var cookie = new Mock<IRequestCookieCollection>();
            cookie.Setup(c => c["Authentication"]);

            var httpContext = new Mock<HttpContext>();
            httpContext.Setup(h => h.Request.Cookies).Returns(cookie.Object);

            await this._filter.OnAuthorizationAsync(context);

            Assert.IsType<ContentResult>(context.Result);
            var result = context.Result as ContentResult;
            Assert.Equal(StatusCodes.Status401Unauthorized, result.StatusCode);
        }
    }
}
