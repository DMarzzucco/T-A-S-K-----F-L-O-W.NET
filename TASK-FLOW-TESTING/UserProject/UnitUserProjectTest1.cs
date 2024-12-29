using Microsoft.AspNetCore.Mvc;
using Moq;
using TASK_FLOW.NET.UserProject.Controllers;
using TASK_FLOW.NET.UserProject.Model;
using TASK_FLOW.NET.UserProject.Services.Interface;
using TASK_FLOW_TESTING.UserProject.Mocks;
namespace TASK_FLOW_TESTING.UserProject
{
    public class UnitUserProjectTest1
    {
        private readonly Mock<IUserProjectService> _service;
        private readonly UserProjectController _controller;

        public UnitUserProjectTest1()
        {
            this._service = new Mock<IUserProjectService>();
            this._controller = new UserProjectController(this._service.Object);
        }
        /// <summary>
        /// Register One Relation between a User with a Project
        /// </summary>
        /// <returns></returns>
        [Fact]
        public async Task Should_Register_A_User_With_A_Project()
        {
            var body = UserProjectsMocks.CreateUserProjectDTOMOck;
            var up = UserProjectsMocks.MockUserProject;

            this._service.Setup(s => s.CreateUP(body)).ReturnsAsync(up);
            var result = await this._controller.RegisterUserAndProject(body) as OkObjectResult;

            Assert.NotNull(result);
            Assert.Equal(200, result.StatusCode);
            Assert.Equal(up, result.Value);
        }
        /// <summary>
        /// Get all Register of User Project
        /// </summary>
        /// <returns></returns>
        [Fact]
        public async Task Should_Get_All_Register_Of_User_Project()
        {
            var listProjects = new List<UserProjectModel>
            {
               UserProjectsMocks.MockUserProject,
               UserProjectsMocks.MockUserProject,
            };
            this._service.Setup(s => s.ListOfAllUP()).ReturnsAsync(listProjects);
            var res = await this._controller.GetALLUP();
            var result = Assert.IsType<OkObjectResult>(res.Result);

            Assert.NotNull(result);
            Assert.Equal(200, result.StatusCode);
            Assert.Equal(listProjects, result.Value);
        }
        /// <summary>
        /// Get one register of User project
        /// </summary>
        /// <returns></returns>
        [Fact]
        public async Task Should_Get_One_Register_Of_User_Project()
        {
            var up = UserProjectsMocks.MockUserProject;
            var id = 1;
            this._service.Setup(s => s.GetUPbyID(id)).ReturnsAsync(up);
            var res = await this._controller.GetUPbyId(id);
            var result = Assert.IsType<OkObjectResult>(res.Result);

            Assert.NotNull(result);
            Assert.Equal(200, result.StatusCode);
            Assert.Equal(up, result.Value);
        }
        /// <summary>
        /// Update One relation between a user with a project
        /// </summary>
        /// <returns></returns>
        [Fact]
        public async Task Should_Update_One_Relation_Between_User_With_A_Project()
        {
            var body = UserProjectsMocks.UpdateUserProjectDTOMOck;
            var up = UserProjectsMocks.MockUserProject;
            var id = 1;
            this._service.Setup(s => s.UpdateUP(id, body)).ReturnsAsync(up);

            var res = await this._controller.UpdateUP(id, body) as NoContentResult;
            Assert.NotNull(res);
            Assert.Equal(204, res.StatusCode);
        }
    }
}