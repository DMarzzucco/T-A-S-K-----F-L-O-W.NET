using AutoMapper;
using Moq;
using TASK_FLOW.NET.Auth.Service.Interface;
using TASK_FLOW.NET.Project.DTO;
using TASK_FLOW.NET.Project.Model;
using TASK_FLOW.NET.Project.Repository.Interface;
using TASK_FLOW.NET.Project.Service;
using TASK_FLOW.NET.UserProject.DTO;
using TASK_FLOW.NET.UserProject.Model;
using TASK_FLOW.NET.UserProject.Repository.Interface;
using TASK_FLOW_TESTING.Project.Mock;
using TASK_FLOW_TESTING.User.Mocks;

namespace TASK_FLOW_TESTING.Project
{
    public class IntProjectTest
    {
        private readonly Mock<IProjectRepository> _repository;
        private readonly Mock<IAuthService> _authService;
        private readonly Mock<IUserProjectRepository> _userProjectRepository;
        private readonly IMapper _mapper;
        private readonly ProjectService _projectService;

        public IntProjectTest()
        {
            this._repository = new Mock<IProjectRepository>();
            this._authService = new Mock<IAuthService>();
            this._userProjectRepository = new Mock<IUserProjectRepository>();

            var config = new MapperConfiguration(conf =>
            {
                conf.CreateMap<CreateProjectDTO, ProjectModel>();
                conf.CreateMap<UpdateProjectDTO, ProjectModel>();
                conf.CreateMap<UserProjectDTO, UserProjectModel>();
            });
            this._mapper = config.CreateMapper();

            this._projectService = new ProjectService(
                this._repository.Object,
                this._mapper,
                this._authService.Object,
                this._userProjectRepository.Object
                );
        }

        /// <summary>
        /// Create Project
        /// </summary>
        /// <returns></returns>
        [Fact]
        public async Task Should_Create_A_Project()
        {
            var user = UsersMock.UserMock;
            this._authService.Setup(u => u.GetUserByCookie()).ReturnsAsync(user);

            var body = ProjectMock.CreateProjectDTOMock;
            var project = ProjectMock.ProjectModelMock;

            this._mapper.Map<ProjectModel>(body);

            this._repository.Setup(r => r.SaveProjectAsync(It.IsAny<ProjectModel>())).Returns(Task.CompletedTask);

            this._userProjectRepository.Setup(r => r.AddChangeAsync(It.IsAny<UserProjectModel>())).Returns(Task.CompletedTask);

            var res = await this._projectService.SaveProject(body);
            var result = Assert.IsType<UserProjectModel>(res);

            Assert.NotNull(result);
            Assert.Equal(user, result.User);
            Assert.Equal(project.Name, result.Project.Name);
        }

        /// <summary>
        /// Get all Project
        /// </summary>
        /// <returns></returns>
        [Fact]
        public async Task Should_List_Of_All_Project()
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

            this._repository.Setup(r => r.ListOfProjectAsync()).ReturnsAsync(listProjects);

            var res = await this._projectService.ListOfProject();
            var result = Assert.IsType<List<ProjectModel>>(res);

            Assert.NotNull(result);
            Assert.Equal(listProjects, result);
        }

        /// <summary>
        /// Get a Project by id 
        /// </summary>
        /// <returns></returns>
        [Fact]
        public async Task Should_Get_A_Project_By_Id()
        {
            var project = ProjectMock.ProjectModelMock;

            var projectId = 4;

            this._repository.Setup(r => r.findByIdAsync(projectId)).ReturnsAsync(project);
            var res = await this._projectService.GetProjectById(projectId);
            var result = Assert.IsType<ProjectModel>(res);

            Assert.NotNull(result);
            Assert.Equal(project.Id, result.Id);
        }

        /// <summary>
        /// Update Project
        /// </summary>
        /// <returns></returns>
        [Fact]
        public async Task Should_Update_A_Project()
        {
            var body = ProjectMock.UpdateProjectDTOMock;
            var project = ProjectMock.ProjectModelMock;

            var projectId = 4;

            this._repository.Setup(r => r.findByIdAsync(projectId)).ReturnsAsync(project);

            this._repository.Setup(r => r.UpdateProjectAsync(It.IsAny<ProjectModel>())).Returns(Task.CompletedTask);

            var res = await this._projectService.UpdateProject(projectId, body);
            var result = Assert.IsType<ProjectModel>(res);

            Assert.NotNull(result);
            Assert.Equal(project.Id, result.Id);
            Assert.Equal(project, result);
        }

        /// <summary>
        /// Delete a project
        /// </summary>
        /// <returns></returns>
        [Fact]
        public async Task Shoudl_Delete_A_Project()
        {
            var project = ProjectMock.ProjectModelMock;

            var projectId = 4;
            this._repository.Setup(r => r.findByIdAsync(projectId)).ReturnsAsync(project);

            this._repository.Setup(r => r.DeleteProjectAsync(It.IsAny<ProjectModel>())).Returns(Task.CompletedTask);

            var res = this._projectService.DeleteProject(projectId);
            Assert.NotNull(res);
        }
    }


}
