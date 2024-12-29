using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using TASK_FLOW.NET.Auth.Controller;
using TASK_FLOW.NET.Auth.Service.Interface;
using TASK_FLOW_TESTING.Auth.Mock;
using TASK_FLOW_TESTING.User.Mocks;

namespace TASK_FLOW_TESTING.Auth
{
    public class UnitAuthTest1
    {
        private readonly Mock<IAuthService> _mockService;
        private readonly AuthController _authController;

        public UnitAuthTest1()
        {
            this._mockService = new Mock<IAuthService>();
            this._authController = new AuthController(this._mockService.Object);
        }

        [Fact]
        public async Task Login_ShouldReturnToken_WhenUserIsValid()
        {
            var authProps = AuthMock.AuthDTOMock;
            var user = UsersMock.UserMock;

            var token = AuthMock.TokenMock.AccessToken;

            var httpContext = new DefaultHttpContext();
            httpContext.Items["User"] = user;

            this._authController.ControllerContext.HttpContext = httpContext;
            this._mockService.Setup(s => s.GenerateToken(user)).ReturnsAsync(token);

            var result = await this._authController.Login(authProps) as ObjectResult;

            Assert.NotNull(result);
            Assert.Equal(StatusCodes.Status200OK, result.StatusCode);
        }

        [Fact]
        public async Task Get_the_Username_of_User()
        {
            var expectUser = "DMarzz";
            this._mockService.Setup(s => s.GetProfile()).ReturnsAsync(expectUser);

            var result = await this._authController.GetProfile();

            var okResult = Assert.IsType<OkObjectResult>(result.Result);

            Assert.Equal(StatusCodes.Status200OK, okResult.StatusCode);
            Assert.Equal(expectUser, okResult.Value);
        }

        [Fact]
        public async Task LogOut()
        {
            this._mockService.Setup(s => s.Logout()).Returns(Task.CompletedTask);

            var result = await this._authController.LogOut() as OkObjectResult;

            Assert.NotNull(result);
            Assert.Equal(StatusCodes.Status200OK, result.StatusCode);
        }
    }
}