using Microsoft.AspNetCore.Http;
using Moq;
using TASK_FLOW.NET.Auth.Cookie.Service.Interface;
using TASK_FLOW.NET.Auth.JWT.Service.Interface;
using TASK_FLOW.NET.Auth.Middleware;
using TASK_FLOW.NET.Auth.Service.Interface;
using TASK_FLOW_TESTING.Auth.Mock;
using TASK_FLOW_TESTING.User.Mocks;

namespace TASK_FLOW_TESTING.Auth.Middleware
{
   
    public class UnitRefreshTokenMiddlewareTest
    {
        private readonly Mock<ITokenService> _tokenSerivce;
        private readonly Mock<IAuthService> _authService;
        private readonly Mock<ICookieService> _cookieService;
        private readonly RequestDelegate _next;

        public UnitRefreshTokenMiddlewareTest()
        {
            this._tokenSerivce = new Mock<ITokenService>();
            this._authService = new Mock<IAuthService>();
            this._cookieService = new Mock<ICookieService>();
            this._next = (HttpContext context) => Task.CompletedTask;
        }
        // Allow Public Path
        [Fact]
        public async Task ShouldAllowPublicPathAndConitnue()
        {
            var context = new DefaultHttpContext();
            context.Request.Path = "/api/Auth/login";

            var middleware = new RefreshTokenMiddleware(this._next);

            await middleware.InvokeAsync(context, this._tokenSerivce.Object, this._authService.Object, this._cookieService.Object);

            Assert.Equal(StatusCodes.Status200OK, context.Response.StatusCode);
        }
        // Return 401 when access token is missing 
        [Fact]
        public async Task ShouldReturn401WhenAccessTokenIsMissing()
        {
            var context = new DefaultHttpContext();

            var middleware = new RefreshTokenMiddleware(this._next);

            await middleware.InvokeAsync(context, this._tokenSerivce.Object, this._authService.Object, this._cookieService.Object);

            Assert.Equal(StatusCodes.Status401Unauthorized, context.Response.StatusCode);
        }

        // Return 403 when access token is invalid 
        [Fact]
        public async Task ShouldReturn403WhenAccessTokenIsInvalid()
        {
            var context = new DefaultHttpContext();

            context.Request.Headers["Cookie"] = "Authentication=invalidToken";

            this._tokenSerivce.Setup(s => s.ValidateToken("invalid")).Returns(false);

            var middleware = new RefreshTokenMiddleware(this._next);

            await middleware.InvokeAsync(context, this._tokenSerivce.Object, this._authService.Object, this._cookieService.Object);

            Assert.Equal(StatusCodes.Status403Forbidden, context.Response.StatusCode);
        }

        // Refresh Token when expiration is soon 
        [Fact]
        public async Task Should_Refresh_Token_When_Expiration_Is_Soon()
        {
            // Arrange
            var context = new DefaultHttpContext();
            var token = AuthMock.TokenMock;
            context.Request.Headers["Cookie"] = "Authentication=validToken; RefreshToken=validRefreshToken";
            var user = UsersMock.UserMock;

            this._tokenSerivce.Setup(ts => ts.ValidateToken("validToken")).Returns(true);
            this._tokenSerivce.Setup(ts => ts.isTokenExpireSoon("validToken")).Returns(true);
            this._tokenSerivce.Setup(ts => ts.ValidateToken("validRefreshToken")).Returns(true);

            this._authService.Setup(s => s.GetUserByCookie()).ReturnsAsync(user);

            this._tokenSerivce.Setup(ts => ts.RefreshToken(user)).Returns(token);

            var middleware = new RefreshTokenMiddleware(_next);

            // Act
            await middleware.InvokeAsync(context, this._tokenSerivce.Object, this._authService.Object, this._cookieService.Object);

            // Assert
            this._cookieService.Verify(cs => cs.SetTokenCookies(It.IsAny<HttpResponse>(), token), Times.Once);
        }


        // Return 401 when refresh token is missing
        [Fact]
        public async Task ShouldReturn401WhenRefreshTokenIsMissing()
        {
            var context = new DefaultHttpContext();

            var middleware = new RefreshTokenMiddleware(this._next);

            await middleware.InvokeAsync(context, this._tokenSerivce.Object, this._authService.Object, this._cookieService.Object);

            Assert.Equal(StatusCodes.Status401Unauthorized, context.Response.StatusCode);
        }

        // Return 403 when refresh token is invalid 
        [Fact]
        public async Task ShouldReturn403WhenRefreshTokenIsInvalid()
        {
            var context = new DefaultHttpContext();

            context.Request.Headers["Cookie"] = "Authentication=invalidToken";


            this._tokenSerivce.Setup(s => s.ValidateToken("invalid")).Returns(false);

            var middleware = new RefreshTokenMiddleware(this._next);

            await middleware.InvokeAsync(context, this._tokenSerivce.Object, this._authService.Object, this._cookieService.Object);

            Assert.Equal(StatusCodes.Status403Forbidden, context.Response.StatusCode);
        }
    }
}
