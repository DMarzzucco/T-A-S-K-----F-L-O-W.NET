using AutoMapper;
using Moq;
using TASK_FLOW.NET.Project.Service.Interface;
using TASK_FLOW.NET.Tasks.DTOs;
using TASK_FLOW.NET.Tasks.Model;
using TASK_FLOW.NET.Tasks.Repository.Interface;
using TASK_FLOW.NET.Tasks.Service;
using TASK_FLOW_TESTING.Project.Mock;
using TASK_FLOW_TESTING.Tasks.Mock;

namespace TASK_FLOW_TESTING.Tasks
{
    public class InterTaskTest
    {
        private readonly Mock<ITaskRepository> _repository;
        private readonly Mock<IProjectService> _projectService;
        private readonly IMapper _mapper;
        private readonly TaskService _service;

        public InterTaskTest()
        {
            this._repository = new Mock<ITaskRepository>();
            this._projectService = new Mock<IProjectService>();
            var conf = new MapperConfiguration(conf =>
            {
                conf.CreateMap<CreateTaskDTO, TaskModel>();
                conf.CreateMap<UpdateTaskDTO, TaskModel>();
            });
            this._mapper = conf.CreateMapper();
            this._service = new TaskService(
                this._repository.Object,
                this._mapper,
                this._projectService.Object
                );
        }

        /// <summary>
        /// Create Task
        /// </summary>
        /// <returns></returns>
        [Fact]
        public async Task ShouldCreateTask()
        {
            var body = TasksMocks.CreateTaskDTOMosck;
            var tasks = TasksMocks.TasksModelMosck;
            var projectId = 4;
            var project = ProjectMock.ProjectModelMock;

            this._projectService.Setup(s => s.GetProjectById(projectId)).ReturnsAsync(project);

            this._repository.Setup(r => r.SaveTaskAsync(tasks)).ReturnsAsync(true);

            var res = await this._service.CreateTask(projectId, body) as TaskModel;
            Assert.NotNull(res);
            Assert.Equal(tasks.Name, res.Name);
        }
        /// <summary>
        /// Get all Task
        /// </summary>
        /// <returns></returns>
        [Fact]
        public async Task ShouldGetAllTask()
        {
            var list = new List<TaskModel> {
                TasksMocks.TasksModelMosck,
                TasksMocks.TasksModelMosck
            };
            this._repository.Setup(r => r.ListAllTaskAsync()).ReturnsAsync(list);
            var res = await this._service.ListOfAllTask() as IEnumerable<TaskModel>;
            Assert.NotNull(res);
            Assert.Equal(list, res);
        }
        /// <summary>
        /// Get One Task
        /// </summary>
        /// <returns></returns>
        [Fact]
        public async Task ShouldGetOneTask()
        {
            var tasks = TasksMocks.TasksModelMosck;
            var id = 1;
            this._repository.Setup(r => r.findByIdAsync(id)).ReturnsAsync(tasks);

            var res = await this._service.GetTaskById(id) as TaskModel;
            Assert.NotNull(res);
            Assert.Equal(tasks, res);
            Assert.Equal(tasks.Id, res.Id);
        }
        /// <summary>
        /// Update one task
        /// </summary>
        /// <returns></returns>
        [Fact]
        public async Task ShouldUpdateOneTask()
        {
            var tasks = TasksMocks.TasksModelMosck;
            var id = 1;
            var body = TasksMocks.UpdateTaskDTOMosck;
            this._repository.Setup(r => r.findByIdAsync(id)).ReturnsAsync(tasks);
            this._repository.Setup(r => r.UpdateTaskAsync(tasks)).Returns(Task.CompletedTask);

            var res = await this._service.UpdateTask(id, body) as TaskModel;
            Assert.NotNull(res);
            Assert.Equal(tasks, res);
        }
        /// <summary>
        /// Delete one Task
        /// </summary>
        /// <returns></returns>
        [Fact]
        public async Task ShouldDeleteOneTask() {
            var tasks = TasksMocks.TasksModelMosck;
            var id = 1;
            this._repository.Setup(r => r.findByIdAsync(id)).ReturnsAsync(tasks);
            this._repository.Setup(r => r.DeleteTaskAsync(tasks)).Returns(Task.CompletedTask);
            var res = this._service.DeleteTask(id);

            Assert.NotNull(res);
        }
    }
}
