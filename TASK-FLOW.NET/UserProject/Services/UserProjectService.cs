using AutoMapper;
using TASK_FLOW.NET.Project.Repository.Interface;
using TASK_FLOW.NET.User.Enums;
using TASK_FLOW.NET.User.Repository.Interface;
using TASK_FLOW.NET.UserProject.DTO;
using TASK_FLOW.NET.UserProject.Model;
using TASK_FLOW.NET.UserProject.Repository.Interface;
using TASK_FLOW.NET.UserProject.Services.Interface;

namespace TASK_FLOW.NET.UserProject.Services
{
    public class UserProjectService : IUserProjectService
    {
        private readonly IUserProjectRepository _repository;
        private readonly IMapper _mapper;
        private readonly IProjectRepository _projectRepository;
        private readonly IUserRepository _userRepository;

        public UserProjectService(IUserProjectRepository repository, IMapper mapper, IProjectRepository projectRepository, IUserRepository userRepository)
        {
            _repository = repository;
            _projectRepository = projectRepository;
            _userRepository = userRepository;
            _mapper = mapper;
        }

        public async Task<UserProjectModel> CreateUP(UserProjectDTO body)
        {
            var project = await _projectRepository.findByIdAsync(body.ProjectId);
            if (project == null)
                throw new KeyNotFoundException("Project not found");

            var user = await _userRepository.FindByIdAsync(body.UserId);
            if (user == null)
                throw new KeyNotFoundException("User not found");

            var UP = new UserProjectModel
            {
                AccessLevel = body.AccessLevel,
                User = user,
                Project = project
            };
            await _repository.AddChangeAsync(UP);
            return UP;
        }

        public async Task<UserProjectModel> GetUPbyID(int id)
        {
            var UP = await _repository.findById(id);
            if (UP == null)
                throw new KeyNotFoundException("This relation not was foundet");
            return UP;
        }

        public async Task<IEnumerable<UserProjectModel>> ListOfAllUP()
        {
            return await _repository.ListofAllAsync();
        }

        public async Task<UserProjectModel> UpdateUP(int id, UpdateUserProjectDTO body)
        {
            var up = await _repository.findById(id);
            if (up == null)
                throw new KeyNotFoundException("This relation not was foundet");

            _mapper.Map(body, up);
            await _repository.UpdateUPAsync(up);
            return up;
        }
    }
}
