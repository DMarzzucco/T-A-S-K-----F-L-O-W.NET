using Moq;
using Xunit;
using Microsoft.AspNetCore.Http;
using TASK_FLOW.NET.User.Service.Interface;
using TASK_FLOW.NET.Auth.JWT.Service.Interface;
using TASK_FLOW.NET.Auth.Cookie.Service.Interface;
using TASK_FLOW.NET.Auth.Service;
using TASK_FLOW.NET.User.Model;
using TASK_FLOW.NET.User.Enums;
using TASK_FLOW.NET.Auth.JWT.DTO;
using Microsoft.AspNetCore.Mvc;
using TASK_FLOW.NET.Auth.DTO;
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
            var user = new UsersModel
            {
                First_name = "Dario",
                Last_name = "Marzzucco",
                Age = "27",
                Username = "DMarzz",
                Email = "DMarzz@gmail.com",
                Password = "promotheus98",
                Roles = ROLES.ADMIN
            };
            var token = new TokenPair
            {
                AccessToken = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJzdWIiOiIxIiwicm9sIjoiMSIsIm5iZiI6MTczNTM5MDkzMiwiZXhwIjoxNzM1NTYzNzMyLCJpYXQiOjE3MzUzOTA5MzJ9.fxCAmD20OHRbD28D5PhuVkLkidcySTblRdT0geFQfO4",
                RefreshToken = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJzdWIiOiIxIiwicm9sIjoiMSIsIm5iZiI6MTczNTM5MDkzMiwiZXhwIjoxNzM1ODIyOTMyLCJpYXQiOjE3MzUzOTA5MzJ9.7WoceqK9cqsQvs6KEAymuY8nyU4ElAV_bUBFU8WEacs",

                RefreshTokenHasher = "$2a$11$4oaZ9eM55kz2WkDnazw7s.Uh66Pu/raUH0tue3qqRPd1V6NEJcf/."
            };

            this._tokenService.Setup(x => x.GenerateToken(user)).Returns(token);

            var result = await this._authService.GenerateToken(user);

            this._userService.Verify(x => x.UpdateToken(user.Id, token.RefreshTokenHasher), Times.Once);

            this._cookieService.Verify(x => x.SetTokenCookies(It.IsAny<HttpResponse>(), token), Times.Once);

            Assert.Equal(token.AccessToken, result);
        }

        [Fact]
        public async Task Should_Get_The_User_From_Cookie()
        {
            var user = new UsersModel
            {
                First_name = "Dario",
                Last_name = "Marzzucco",
                Age = "27",
                Username = "DMarzz",
                Email = "DMarzz@gmail.com",
                Password = "promotheus98",
                Roles = ROLES.ADMIN
            };
            this._tokenService.Setup(t => t.GetIdFromToken()).Returns(user.Id);
            this._userService.Setup(u => u.GetById(user.Id)).ReturnsAsync(user);

            var response = this._authService.GetUserByCookie();
            var result = Assert.IsType<UsersModel>(response.Result);

            Assert.Equal(user, result);
        }
        [Fact]
        public async Task Should_Validate_the_User_Credentials()
        {
            var authProps = new AuthPropsDTO
            {
                Username = "DMarzz",
                Password = "promotheus98"
            };
            var user = new UsersModel
            {
                First_name = "Dario",
                Last_name = "Marzzucco",
                Age = "27",
                Username = "DMarzz",
                Email = "DMarzz@gmail.com",
                Password = "AQAAAAIAAYagAAAAEEM5GX6vpkzonyKHRs+sSAa22CshBphstUvLsiBfzwYEDjPr5K9WM4xdXbDM1/v6vw==",
                Roles = ROLES.ADMIN
            };
            this._userService.Setup(u => u.FindByAuth("Username", authProps.Username)).ReturnsAsync(user);

            var result = this._authService.ValidationUser(authProps);

            Assert.Equal(user, result.Result);
        }
    }
}
