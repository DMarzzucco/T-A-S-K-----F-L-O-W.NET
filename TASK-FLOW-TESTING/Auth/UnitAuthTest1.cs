using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using TASK_FLOW.NET.Auth.Controller;
using TASK_FLOW.NET.Auth.DTO;
using TASK_FLOW.NET.Auth.Service.Interface;
using TASK_FLOW.NET.User.Enums;
using TASK_FLOW.NET.User.Model;

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
                Password = "promotheus98",
                Roles = ROLES.ADMIN
            };
            var token = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJzdWIiOiIxIiwicm9sIjoiMSIsIm5iZiI6MTczNTM5MDkzMiwiZXhwIjoxNzM1NTYzNzMyLCJpYXQiOjE3MzUzOTA5MzJ9.fxCAmD20OHRbD28D5PhuVkLkidcySTblRdT0geFQfO4";

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