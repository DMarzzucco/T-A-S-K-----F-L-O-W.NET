using AutoMapper;
using TASK_FLOW.NET.Project.Repository.Interface;
using TASK_FLOW.NET.User.DTO;
using TASK_FLOW.NET.User.Enums;
using TASK_FLOW.NET.User.Model;
using TASK_FLOW.NET.User.Repository.Interface;
using TASK_FLOW.NET.User.Service.Interface;

namespace TASK_FLOW.NET.User.Service
{
    public class UserProjectService : IUserProjectService
    {
        private readonly IUserProjectRepository _repository;
        private readonly IMapper _mapper;
        private readonly IProjectRepository _projectRepository;
        private readonly IUserRepository _userRepository;

        public UserProjectService(IUserProjectRepository repository, IMapper mapper, IProjectRepository projectRepository, IUserRepository userRepository)
        {
            this._repository = repository;
            this._projectRepository = projectRepository;
            this._userRepository = userRepository;
            this._mapper = mapper;
        }

        public async Task<UserProjectModel> CreateUP(UserProjectDTO body)
        {
            var project = await this._projectRepository.findByIdAsync(body.ProjectId);
            if (project == null)
                throw new KeyNotFoundException("Project not found");

            var user = await this._userRepository.FindByIdAsync(body.UserId);
            if (user == null)
                throw new KeyNotFoundException("User not found");

            var UP = new UserProjectModel
            {
                AccessLevel = body.AccessLevel,
                User = user,
                Project = project
            };
            await this._repository.AddChangeAsync(UP);
            return UP;
        }

        public async Task<UserProjectModel> GetUPbyID(int id)
        {
            var UP = await this._repository.findById(id);
            if (UP == null)
                throw new KeyNotFoundException("This relation not was foundet");
            return UP;
        }

        public async Task<IEnumerable<UserProjectModel>> ListOfAllUP()
        {
            return await this._repository.ListofAllAsync();
        }

        public async Task<UserProjectModel> UpdateUP(int id, UpdateUserProjectDTO body)
        {
            var up = await this._repository.findById(id);
            if (up == null)
                throw new KeyNotFoundException("This relation not was foundet");

            this._mapper.Map(body, up);
            await this._repository.UpdateUPAsync(up);
            return up;
        }
    }
}
