using Microsoft.AspNetCore.Mvc;
using Moq;
using TASK_FLOW.NET.Tasks.Controller;
using TASK_FLOW.NET.Tasks.Model;
using TASK_FLOW.NET.Tasks.Service.Interface;
using TASK_FLOW_TESTING.Tasks.Mock;

namespace TASK_FLOW_TESTING.Tasks
{
    public class UnitTaskTest1
    {
        private readonly Mock<ITaskService> _services;
        private readonly TaskController _controller;

        public UnitTaskTest1()
        {
            this._services = new Mock<ITaskService>();
            this._controller = new TaskController(this._services.Object);
        }

        /// <summary>
        /// Create a new Task
        /// </summary>
        /// <returns></returns>
        [Fact]
        public async Task ShouldCreateOneTask()
        {
            var tasks = TasksMocks.TasksModelMosck;
            var body = TasksMocks.CreateTaskDTOMosck;
            var projectID = 4;
            this._services.Setup(s => s.CreateTask(projectID, body)).ReturnsAsync(tasks);

            var res = await this._controller.CreateTask(projectID, body);
            var result = Assert.IsType<CreatedAtActionResult>(res.Result);

            Assert.NotNull(result);
            Assert.Equal(201, result.StatusCode);
            Assert.Equal(tasks, result.Value);
        }
        /// <summary>
        /// Get All Task Register
        /// </summary>
        /// <returns></returns>
        [Fact]
        public async Task ShouldGetAllTaskRegisters()
        {
            var list = new List<TaskModel> {
                TasksMocks.TasksModelMosck,
                TasksMocks.TasksModelMosck
            };
            this._services.Setup(s => s.ListOfAllTask()).ReturnsAsync(list);
            var res = await this._controller.GetAllTAsk();
            var result = Assert.IsType<OkObjectResult>(res.Result);

            Assert.NotNull(result);
            Assert.Equal(200, result.StatusCode);
            Assert.Equal(list, result.Value);
        }
        /// <summary>
        /// Get One Task Register
        /// </summary>
        /// <returns></returns>
        [Fact]
        public async Task ShouldGetOneTaskRegisters()
        {
            var task = TasksMocks.TasksModelMosck;
            var id = 1;
            this._services.Setup(s => s.GetTaskById(id)).ReturnsAsync(task);

            var res = await this._controller.GetTaskById(id);
            var result = Assert.IsType<OkObjectResult>(res.Result);

            Assert.NotNull(result);
            Assert.Equal(200, result.StatusCode);
            Assert.Equal(task, result.Value);
        }
        /// <summary>
        /// Update one Task Register
        /// </summary>
        /// <returns></returns>
        [Fact]
        public async Task ShouldUpdateOneTaskRegisters()
        {
            var body = TasksMocks.UpdateTaskDTOMosck;
            var id = 1;
            var task = TasksMocks.TasksModelMosck;
            this._services.Setup(s => s.UpdateTask(id, body)).ReturnsAsync(task);
            var res = await this._controller.UpdateTask(id, body) as NoContentResult;
            Assert.NotNull(res);
            Assert.Equal(204, res.StatusCode);
        }
        /// <summary>
        /// Delete One task Register
        /// </summary>
        /// <returns></returns>
        [Fact]
        public async Task ShouldDeleteOneTaskRegisters()
        {
            var id = 1;
            this._services.Setup(s => s.DeleteTask(id)).Returns(Task.CompletedTask);
            var res = await this._controller.DeleteTask(id) as NoContentResult;
            Assert.NotNull(res);
            Assert.Equal(204, res.StatusCode);
        }
    }
}