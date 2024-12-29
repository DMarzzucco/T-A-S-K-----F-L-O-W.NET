using Moq;
using Xunit;
using Microsoft.AspNetCore.Http;
using TASK_FLOW.NET.User.Service.Interface;
using TASK_FLOW.NET.Auth.JWT.Service.Interface;
using TASK_FLOW.NET.Auth.Cookie.Service.Interface;
using TASK_FLOW.NET.Auth.Service;
using TASK_FLOW.NET.User.Model;
using TASK_FLOW_TESTING.User.Mocks;
using TASK_FLOW_TESTING.Auth.Mock;
namespace TASK_FLOW_TESTING.Auth
{
    public class InterAuthTest1
    {
        private readonly Mock<IUserService> _userService;
        private readonly Mock<IHttpContextAccessor> _httpContext;
        private readonly Mock<ITokenService> _tokenService;
        private readonly Mock<ICookieService> _cookieService;
        private readonly AuthService _authService;

        public InterAuthTest1()
        {
            this._userService = new Mock<IUserService>();
            this._httpContext = new Mock<IHttpContextAccessor>();
            this._tokenService = new Mock<ITokenService>();
            this._cookieService = new Mock<ICookieService>();

            this._httpContext.Setup(x => x.HttpContext).Returns(new DefaultHttpContext());

            this._authService = new AuthService(
                this._userService.Object,
                this._httpContext.Object,
                this._tokenService.Object,
                this._cookieService.Object
                );
        }

        [Fact]
        public async Task GenerateToken_ShouldGenerateTokenAndSetCookie()
        {
            var user = UsersMock.UserMock;
            var token = AuthMock.TokenMock;

            this._tokenService.Setup(x => x.GenerateToken(user)).Returns(token);

            var result = await this._authService.GenerateToken(user);

            this._userService.Verify(x => x.UpdateToken(user.Id, token.RefreshTokenHasher), Times.Once);

            this._cookieService.Verify(x => x.SetTokenCookies(It.IsAny<HttpResponse>(), token), Times.Once);

            Assert.Equal(token.AccessToken, result);
        }

        [Fact]
        public async Task Should_Get_The_User_From_Cookie()
        {
            var user = UsersMock.UserMock;

            this._tokenService.Setup(t => t.GetIdFromToken()).Returns(user.Id);
            this._userService.Setup(u => u.GetById(user.Id)).ReturnsAsync(user);

            var response = this._authService.GetUserByCookie();
            var result = Assert.IsType<UsersModel>(response.Result);

            Assert.Equal(user, result);
        }
        [Fact]
        public async Task Should_Validate_the_User_Credentials()
        {
            var authProps = AuthMock.AuthDTOMock;
            var user = UsersMock.UserHashPassMock;

            this._userService.Setup(u => u.FindByAuth("Username", authProps.Username)).ReturnsAsync(user);

            var result = this._authService.ValidationUser(authProps);

            Assert.Equal(user, result.Result);
        }
    }
}
