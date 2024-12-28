using Microsoft.AspNetCore.Mvc;
using Moq;
using TASK_FLOW.NET.Project.Controller;
using TASK_FLOW.NET.Project.DTO;
using TASK_FLOW.NET.Project.Model;
using TASK_FLOW.NET.Project.Service.Interface;
using TASK_FLOW.NET.User.Enums;
using TASK_FLOW.NET.User.Model;
using TASK_FLOW.NET.UserProject.Enums;
using TASK_FLOW.NET.UserProject.Model;

namespace TASK_FLOW_TESTING.Project
{
    public class UnitProjectTest1
    {
        private readonly Mock<IProjectService> _projectService;
        private readonly ProjectController _projectController;

        public UnitProjectTest1()
        {
            this._projectService = new Mock<IProjectService>();
            this._projectController = new ProjectController(this._projectService.Object);
        }

        /// <summary>
        /// Create project
        /// </summary>
        /// <returns></returns>
        [Fact]
        public async Task Should_Create_A_Project()
        {
            var body = new CreateProjectDTO
            {
                Name = "Project test",
                Description = "This is a project test"
            };
            var project = new ProjectModel
            {
                Id = 4,
                Name = "Project test",
                Description = "This is a project test"
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
            var userProject = new UserProjectModel
            {
                AccessLevel = ACCESSLEVEL.OWNER,
                User = user,
                Project = project
            };

            this._projectService.Setup(s => s.SaveProject(body)).ReturnsAsync(userProject);

            var res = await this._projectController.CreateProject(body);
            var result = Assert.IsType<CreatedAtActionResult>(res.Result);

            Assert.NotNull(result);
            Assert.Equal(201, result.StatusCode);
            Assert.Equal("GetAllProject", result.ActionName);
            Assert.Equal(userProject, result.Value);
        }

        /// <summary>
        /// Get all Project
        /// </summary>
        /// <returns></returns>
        [Fact]
        public async Task GetAllProject_ShouldReturnOkWithProjects()
        {
            var listProjects = new List<ProjectModel>
            {
                new ProjectModel
            {
                Id = 4,
                Name = "Project test",
                Description = "This is a project test"
            },
                new ProjectModel
            {
                Id = 4,
                Name = "Project test",
                Description = "This is a project test"
            }
            };

            this._projectService.Setup(p => p.ListOfProject()).ReturnsAsync(listProjects);

            var res = await this._projectController.GetAllProject();
            var result = Assert.IsType<OkObjectResult>(res.Result);

            Assert.NotNull(result);
            Assert.Equal(200, result.StatusCode);
            Assert.Equal(listProjects, result.Value);
        }

        /// <summary>
        /// Get a project according his id
        /// </summary>
        /// <returns></returns>
        [Fact]
        public async Task Get_A_Project_By_His_Id()
        {
            var project = new ProjectModel
            {
                Id = 4,
                Name = "Project test",
                Description = "This is a project test"
            };
            var projectId = 4;

            this._projectService.Setup(p => p.GetProjectById(projectId)).ReturnsAsync(project);

            var res = await this._projectController.GetProjectById(projectId);
            var result = Assert.IsType<OkObjectResult>(res.Result);

            Assert.NotNull(result);
            Assert.Equal(200, result.StatusCode);
            Assert.Equal(project, result.Value);
        }

        /// <summary>
        /// Update Project
        /// </summary>
        /// <returns></returns>
        [Fact]
        public async Task Should_Update_Project()
        {
            var body = new UpdateProjectDTO
            {
                Name = "Update ",
                Description = "This is a Update test "
            };
            var project = new ProjectModel
            {
                Id = 4,
                Name = "Project test",
                Description = "This is a project test"
            };
            var projectId = 4;
            this._projectService.Setup(p => p.UpdateProject(projectId, body)).ReturnsAsync(project);

            var res = await this._projectController.UpdateProject(projectId, body);
            var result = Assert.IsType<NoContentResult>(res.Result);

            Assert.NotNull(result);
            Assert.Equal(204, result.StatusCode);
        }

        /// <summary>
        /// Delete Project
        /// </summary>
        /// <returns></returns>
        [Fact]
        public async Task Should_Delte_A_PRoject()
        {
            var projectId = 4;

            this._projectService.Setup(p => p.DeleteProject(projectId)).Returns(Task.CompletedTask);

            var res = await this._projectController.DeleteProject(projectId);
            var result = Assert.IsType<NoContentResult>(res);

            Assert.NotNull(result);
            Assert.Equal(204, result.StatusCode);
        }
    }
}