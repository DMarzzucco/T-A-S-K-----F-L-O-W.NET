using Microsoft.AspNetCore.Mvc;
using TASK_FLOW.NET.Auth.Attributes;
using TASK_FLOW.NET.Tasks.DTOs;
using TASK_FLOW.NET.Tasks.Model;
using TASK_FLOW.NET.Tasks.Service.Interface;
using TASK_FLOW.NET.User.Enums;
using TASK_FLOW.NET.UserProject.Enums;

namespace TASK_FLOW.NET.Tasks.Controller
{
    [Route("api/[controller]")]
    [JwtAuth]
    [AuthRoles]
    [AuthAccessLevel]
    [ApiController]
    public class TaskController : ControllerBase
    {
        private readonly ITaskService _service;

        public TaskController(ITaskService service)
        {
            this._service = service;
        }

        /// <summary>
        /// Create a new Task
        /// </summary>
        /// <param name="ProjectId"></param>
        /// <param name="body"></param>
        /// <returns>Save a task</returns>
        /// <response code = "201">Created</response>
        /// <response code = "400">Bad Request</response>
        [Roles(ROLES.BASIC)]
        [AccessLevel(ACCESSLEVEL.MAINTAINER)]
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<TaskModel>> CreateTask(int ProjectId, [FromBody] CreateTaskDTO body)
        {
            var newTask = await this._service.CreateTask(ProjectId, body);
            return CreatedAtAction(nameof(GetAllTAsk), new { id = newTask.Id }, newTask);
        }

        /// <summary>
        /// Get all Tasks
        /// </summary>
        /// <returns>List of all tasks</returns>
        /// <response code = "200">Ok</response>
        /// <response code = "400">Bad Request</response>
        [Roles(ROLES.BASIC)]
        [AccessLevel(ACCESSLEVEL.OWNER)]
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<IEnumerable<TaskModel>>> GetAllTAsk()
        {
            return Ok(await this._service.ListOfAllTask());
        }

        /// <summary>
        /// Get task by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Return a task according his Id number</returns>
        /// <response code = "200">Ok</response>
        /// <response code = "404">Not found</response>
        [Roles(ROLES.BASIC)]
        [AccessLevel(ACCESSLEVEL.MAINTAINER)]
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<TaskModel>> GetTaskById(int id)
        {
            return Ok(await this._service.GetTaskById(id));
        }

        /// <summary>
        /// Update Task
        /// </summary>
        /// <param name="id"></param>
        /// <param name="body"></param>
        /// <returns>Nothing</returns>
        /// <response code = "204">No content</response>
        /// <response code = "404">Not found</response>
        [Roles(ROLES.BASIC)]
        [AccessLevel(ACCESSLEVEL.MAINTAINER)]
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> UpdateTask(int id, [FromBody] UpdateTaskDTO body)
        {
            await this._service.UpdateTask(id, body);
            return NoContent();
        }

        /// <summary>
        /// Delete task
        /// </summary>
        /// <param name="id"></param>
        /// <returns>No Content</returns>
        /// <response code = "204">No content</response>
        /// <response code = "404">Not found</response>
        [Roles(ROLES.BASIC)]
        [AccessLevel(ACCESSLEVEL.MAINTAINER)]
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteTask(int id)
        {
            await this._service.DeleteTask(id);
            return NoContent();

        }
    }
}
