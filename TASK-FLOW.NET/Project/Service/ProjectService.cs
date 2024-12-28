using AutoMapper;
using TASK_FLOW.NET.Auth.Service.Interface;
using TASK_FLOW.NET.Project.DTO;
using TASK_FLOW.NET.Project.Model;
using TASK_FLOW.NET.Project.Repository.Interface;
using TASK_FLOW.NET.Project.Service.Interface;
using TASK_FLOW.NET.UserProject.Enums;
using TASK_FLOW.NET.UserProject.Model;
using TASK_FLOW.NET.UserProject.Repository.Interface;

namespace TASK_FLOW.NET.Project.Service
{
    public class ProjectService : IProjectService
    {
        private readonly IProjectRepository _repository;
        private readonly IMapper _mapper;
        private readonly IAuthService _authService;
        private readonly IUserProjectRepository _userProjectRepository;

        public ProjectService(IProjectRepository repository, IMapper mapper, IAuthService authService, IUserProjectRepository userProjectRepository)
        {
            this._repository = repository;
            this._authService = authService;
            this._userProjectRepository = userProjectRepository;
            this._mapper = mapper;
        }

        public async Task DeleteProject(int id)
        {
            var project = await this._repository.findByIdAsync(id);
            if (project == null) throw new KeyNotFoundException("Project not found");

            await this._repository.DeleteProjectAsync(project);
        }

        public async Task<ProjectModel> GetProjectById(int id)
        {
            var project = await this._repository.findByIdAsync(id);
            if (project == null) throw new KeyNotFoundException("Project not found");

            return project;
        }

        public async Task<IEnumerable<ProjectModel>> ListOfProject()
        {
            return await this._repository.ListOfProjectAsync();
        }

        public async Task<UserProjectModel> SaveProject(CreateProjectDTO body)
        {
            var user = await this._authService.GetUserByCookie();
            var project = this._mapper.Map<ProjectModel>(body);

            await this._repository.SaveProjectAsync(project);

            var relation = new UserProjectModel
            {
                AccessLevel = ACCESSLEVEL.OWNER,
                User = user,
                Project = project
            };
            var userProject = this._mapper.Map<UserProjectModel>(relation);

            await this._userProjectRepository.AddChangeAsync(userProject);
            return userProject;
        }

        public async Task<ProjectModel> UpdateProject(int id, UpdateProjectDTO body)
        {
            var project = await this._repository.findByIdAsync(id);
            if (project == null) throw new KeyNotFoundException("Project not found");

            this._mapper.Map(body, project);

            await this._repository.UpdateProjectAsync(project);
            return project;
        }
    }
}
