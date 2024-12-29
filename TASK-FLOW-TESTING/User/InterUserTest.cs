using AutoMapper;
using Moq;
using TASK_FLOW.NET.User.DTO;
using TASK_FLOW.NET.User.Model;
using TASK_FLOW.NET.User.Repository.Interface;
using TASK_FLOW.NET.User.Service;
using TASK_FLOW_TESTING.User.Mocks;
namespace TASK_FLOW_TESTING.User
{
    public class InterUserTest
    {
        private readonly Mock<IUserRepository> _repository;
        private readonly IMapper _mapper;
        private readonly UserService _service;

        public InterUserTest()
        {
            this._repository = new Mock<IUserRepository>();

            var conf = new MapperConfiguration(conf =>
            {
                conf.CreateMap<CreateUserDTO, UsersModel>();
                conf.CreateMap<UpdateUserDTO, UsersModel>();
            });
            this._mapper = conf.CreateMapper();

            this._service = new UserService(
                this._repository.Object,
                this._mapper
                );
        }

        /// <summary>
        /// Create a User
        /// </summary>
        /// <returns></returns>
        [Fact]
        public async Task ShouldCreateOneUser()
        {
            var body = UsersMock.CreateUserDTOMOck;
            var user = UsersMock.UserMock;
            this._repository.Setup(r => r.AddChangeAsync(user)).Returns(Task.CompletedTask);

            var res = await this._service.CreateUser(body) as UsersModel;

            Assert.NotNull(res);
            Assert.Equal(user.Username, res.Username);
        }
        /// <summary>
        /// Delete a user
        /// </summary>
        /// <returns></returns>
        [Fact]
        public async Task ShouldDeleteOneUser()
        {
            var id = 1;
            var user = UsersMock.UserMock;

            this._repository.Setup(r => r.FindByIdAsync(id)).ReturnsAsync(user);
            this._repository.Setup(r => r.RemoveAsync(It.IsAny<UsersModel>())).Returns(Task.CompletedTask);

            var res = this._service.DeleteUser(id);

            Assert.NotNull(res);
        }
        /// <summary>
        /// Find on Key about a Value
        /// </summary>
        /// <returns></returns>
        [Fact]
        public async Task ShoulFindByValue()
        {
            var user = UsersMock.UserMock;
            var key = "Username";
            var value = user.Username;

            this._repository.Setup(r => r.FindByKey(key, value)).ReturnsAsync(user);
            var res = await this._service.FindByAuth(key, value);

            Assert.NotNull(res);
            Assert.Equal(value, res.Username);
        }
        /// <summary>
        /// Get all Users
        /// </summary>
        /// <returns></returns>
        [Fact]
        public async Task ShouldGetAllUser()
        {
            var list = new List<UsersModel> {
                UsersMock.UserMock,
                UsersMock.UserMock
            };
            this._repository.Setup(r => r.ToListAsync()).ReturnsAsync(list);
            var res = await this._service.GetAll() as IEnumerable<UsersModel>;

            Assert.NotNull(res);
            Assert.Equal(list, res);
        }

        /// <summary>
        /// Get one user
        /// </summary>
        /// <returns></returns>
        [Fact]
        public async Task ShouldGetOneUser()
        {
            var user = UsersMock.UserMock;
            var id = 4;
            this._repository.Setup(r => r.FindByIdAsync(id)).ReturnsAsync(user);
            var result = await this._service.GetById(id) as UsersModel;
            Assert.NotNull(result);
            Assert.Equal(user, result);
        }
        /// <summary>
        /// Update a user
        /// </summary>
        /// <returns></returns>
        [Fact]
        public async Task ShouldUpdateOneUser() {
            var body = UsersMock.UpdateUserDTOMOck;
            var user = UsersMock.UserMock;
            var id = 4;
            this._repository.Setup(r => r.FindByIdAsync(id)).ReturnsAsync(user);
            this._repository.Setup(r => r.UpdateAsync(user)).Returns(Task.CompletedTask);

            var res = await this._service.UpdateUser(id, body) as UsersModel;

            Assert.NotNull(res);
            Assert.Equal(user, res);
        }
    }
}
