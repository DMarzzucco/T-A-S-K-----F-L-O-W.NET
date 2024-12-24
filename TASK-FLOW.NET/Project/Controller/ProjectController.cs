using Microsoft.AspNetCore.Mvc;
using TASK_FLOW.NET.Project.DTO;
using TASK_FLOW.NET.Project.Model;
using TASK_FLOW.NET.Project.Service.Interface;
using TASK_FLOW.NET.User.Model;

namespace TASK_FLOW.NET.Project.Controller
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProjectController : ControllerBase
    {
        private readonly IProjectService _service;
        public ProjectController(IProjectService service)
        {
            this._service = service;
        }
        /// <summary>
        /// Create Project
        /// </summary>
        /// <param name="body"></param>
        /// <returns>Save a Project</returns>
        /// <response code= "201">Created</response>
        /// <response code= "400">BadRequest</response>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<UserProjectModel>> CreateProject([FromBody] CreateProjectDTO body)
        {
            var project = await this._service.SaveProject(body);
            return Ok(project);
        }

        /// <summary>
        /// Get All Project
        /// </summary>
        /// <returns>List of Project</returns>
        /// <response code= "200">Ok</response>
        /// <response code= "400">Bad Request</response>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<IEnumerable<ProjectModel>>> GetAllProject()
        {
            return Ok(await this._service.ListOfProject());
        }

        /// <summary>
        /// Get project by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Return a project according his id</returns>
        /// <response code= "200">Ok</response>
        /// <response code= "404">Not found</response>
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<ProjectModel>> GetProjectById(int id)
        {
            return Ok(await this._service.GetProjectById(id));
        }

        /// <summary>
        /// Update Project
        /// </summary>
        /// <param name="id"></param>
        /// <param name="body"></param>
        /// <returns>Update a Project</returns>
        /// <response code= "204">No content</response>
        /// <response code= "404">Not found</response>
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<ProjectModel>> UpdateProject(int id, [FromBody] UpdateProjectDTO body)
        {
            await this._service.UpdateProject(id, body);
            return NoContent();
        }

        /// <summary>
        /// Delete Project
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Delete a project</returns>
        /// <response code= "204">No Content</response>
        /// <response code= "404">Not found</response>
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult> DeleteProject(int id)
        {
            await this._service.DeleteProject(id);
            return NoContent();
        }
    }
}
