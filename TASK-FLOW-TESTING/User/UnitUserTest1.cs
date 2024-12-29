using Microsoft.AspNetCore.Mvc;
using Moq;
using TASK_FLOW.NET.User.Controller;
using TASK_FLOW.NET.User.Model;
using TASK_FLOW.NET.User.Service.Interface;
using TASK_FLOW_TESTING.User.Mocks;
namespace TASK_FLOW_TESTING.User
{
    public class UnitUserTest1
    {
        private readonly Mock<IUserService> _service;
        private readonly UserController _controller;

        public UnitUserTest1()
        {
            this._service = new Mock<IUserService>();
            this._controller = new UserController(this._service.Object);
        }
        /// <summary>
        /// Register User
        /// </summary>
        /// <returns></returns>
        [Fact]
        public async Task ShouldRegisterOneUser()
        {
            var body = UsersMock.CreateUserDTOMOck;
            var user = UsersMock.UserMock;

            this._service.Setup(s => s.CreateUser(body)).ReturnsAsync(user);
            var res = await this._controller.RegisterUser(body);
            var result = Assert.IsType<CreatedAtActionResult>(res.Result);

            Assert.NotNull(result);
            Assert.Equal(201, result.StatusCode);
            Assert.Equal(user, result.Value);
        }
        /// <summary>
        /// Get all User Register
        /// </summary>
        /// <returns></returns>
        [Fact]
        public async Task ShouldGetAllUserRegister()
        {
            var list = new List<UsersModel> {
                UsersMock.UserMock,
                UsersMock.UserMock
            };
            this._service.Setup(s => s.GetAll()).ReturnsAsync(list);
            var res = await this._controller.GetAllUser();
            var result = Assert.IsType<OkObjectResult>(res.Result);

            Assert.NotNull(result);
            Assert.Equal(200, result.StatusCode);
            Assert.Equal(list, result.Value);
        }
        /// <summary>
        /// Get one User Register 
        /// </summary>
        /// <returns></returns>
        [Fact]
        public async Task ShouldGetOneUserRegister()
        {
            var user = UsersMock.UserMock;
            var id = 4;

            this._service.Setup(s => s.GetById(id)).ReturnsAsync(user);

            var res = await this._controller.GetUserById(id);
            var result = Assert.IsType<OkObjectResult>(res.Result);

            Assert.NotNull(result);
            Assert.Equal(200, result.StatusCode);
            Assert.Equal(user, result.Value);
        }
        /// <summary>
        /// Update one User Register
        /// </summary>
        /// <returns></returns>
        [Fact]
        public async Task ShouldUpdateOneUserRegister()
        {
            var body = UsersMock.UpdateUserDTOMOck;
            var id = 4;
            var user = UsersMock.UserMock;

            this._service.Setup(s => s.UpdateUser(id, body)).ReturnsAsync(user);

            var res = await this._controller.UpdateUser(id, body) as NoContentResult;

            Assert.NotNull(res);
            Assert.Equal(204, res.StatusCode);
        }
        /// <summary>
        /// Delete one user register
        /// </summary>
        /// <returns></returns>
        [Fact]
        public async Task ShouldDeleteOneUserRegister()
        {
            var id = 4;
            this._service.Setup(s => s.DeleteUser(id)).Returns(Task.CompletedTask);

            var res = await this._controller.DelteUser(id) as NoContentResult;

            Assert.NotNull(res);
            Assert.Equal(204, res.StatusCode);
        }
    }
}