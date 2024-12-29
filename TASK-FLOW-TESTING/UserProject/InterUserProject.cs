using AutoMapper;
using Moq;
using TASK_FLOW.NET.Project.Repository.Interface;
using TASK_FLOW.NET.User.Repository.Interface;
using TASK_FLOW.NET.UserProject.DTO;
using TASK_FLOW.NET.UserProject.Model;
using TASK_FLOW.NET.UserProject.Repository.Interface;
using TASK_FLOW.NET.UserProject.Services;
using TASK_FLOW_TESTING.Project.Mock;
using TASK_FLOW_TESTING.User.Mocks;
using TASK_FLOW_TESTING.UserProject.Mocks;

namespace TASK_FLOW_TESTING.UserProject
{
    public class InterUserProjectTest
    {
        private readonly Mock<IUserProjectRepository> _repository;
        private readonly Mock<IProjectRepository> _projectRepository;
        private readonly Mock<IUserRepository> _userRepository;

        private readonly IMapper _mapper;
        private readonly UserProjectService _service;

        public InterUserProjectTest()
        {
            this._repository = new Mock<IUserProjectRepository>();
            this._projectRepository = new Mock<IProjectRepository>();
            this._userRepository = new Mock<IUserRepository>();

            var config = new MapperConfiguration(conf =>
            {
                conf.CreateMap<UserProjectDTO, UserProjectModel>();
                conf.CreateMap<UpdateUserProjectDTO, UserProjectModel>();
            });
            this._mapper = config.CreateMapper();

            this._service = new UserProjectService(
                this._repository.Object,
                this._mapper,
                this._projectRepository.Object,
                this._userRepository.Object
                );
        }
        /// <summary>
        /// Create Relation UserProject
        /// </summary>
        /// <returns></returns>
        [Fact]
        public async Task ShouldCreateOneRelationBetweenAUserWithAProject() {
            var body = UserProjectsMocks.CreateUserProjectDTOMOck;
            var project = ProjectMock.ProjectModelMock;
            var user = UsersMock.UserMock;

            this._projectRepository.Setup(p => p.findByIdAsync(body.ProjectId)).ReturnsAsync(project);
            this._userRepository.Setup(p => p.FindByIdAsync(body.UserId)).ReturnsAsync(user);
            var up = UserProjectsMocks.MockUserProject;

            this._repository.Setup(r => r.AddChangeAsync(up)).Returns(Task.CompletedTask);
            var res = await this._service.CreateUP(body) as UserProjectModel;

            Assert.NotNull(res);
            Assert.Equal(project, res.Project);
            Assert.Equal(user, res.User);
        }

        /// <summary>
        /// List of All UserProject
        /// </summary>
        /// <returns></returns>
        [Fact]
        public async Task ShouldGetOneListOfALlRegisterOfUserProject() {
            var list = new List<UserProjectModel> {
                UserProjectsMocks.MockUserProject,
                UserProjectsMocks.MockUserProject,
            };
            this._repository.Setup(r => r.ListofAllAsync()).ReturnsAsync(list);
            var res = await this._service.ListOfAllUP() as IEnumerable<UserProjectModel>;

            Assert.NotNull(res);
            Assert.Equal(list, res);
        }

        /// <summary>
        /// Get One Register of User Project 
        /// </summary>
        /// <returns></returns>
        [Fact]
        public async Task ShouldGetOneRegisterOfUserProject() {
            var up = UserProjectsMocks.MockUserProject;
            var id = 1;
            this._repository.Setup(r => r.findById(id)).ReturnsAsync(up);

            var res = await this._service.GetUPbyID(id) as UserProjectModel;

            Assert.NotNull(res);
            Assert.Equal(up.Id, res.Id);
            Assert.Equal(up, res);
        }

        /// <summary>
        /// Edit one Register of User Project
        /// </summary>
        /// <returns></returns>
        [Fact]
        public async Task ShouldEditTheRelationUserProject() {
            var body = UserProjectsMocks.UpdateUserProjectDTOMOck;
            var up = UserProjectsMocks.MockUserProject;
            var id = 1;

            this._repository.Setup(r => r.findById(id)).ReturnsAsync(up);
            this._repository.Setup(r => r.UpdateUPAsync(up)).Returns(Task.CompletedTask);

            var res = await this._service.UpdateUP(id, body) as UserProjectModel;

            Assert.NotNull(res);
            Assert.Equal(up.Id, res.Id);
            Assert.Equal(up, res);
        }
    }
}
