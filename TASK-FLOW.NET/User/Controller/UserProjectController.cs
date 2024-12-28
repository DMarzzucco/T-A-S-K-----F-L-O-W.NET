using Microsoft.AspNetCore.Mvc;
using TASK_FLOW.NET.User.DTO;
using TASK_FLOW.NET.User.Model;
using TASK_FLOW.NET.User.Service.Interface;

namespace TASK_FLOW.NET.User.Controller
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserProjectController : ControllerBase
    {
        private readonly IUserProjectService _service;

        public UserProjectController(IUserProjectService service)
        {
            this._service = service;
        }

        /// <summary>
        /// Relation Project
        /// </summary>
        /// <returns>Relation between a User and a Project with a Access Level</returns>
        /// <response code="200">Relation finished successfully</response>
        /// <response code="400">Relation Faild</response>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult> RegisterUserAndProject([FromBody] UserProjectDTO body)
        {
            var data = await this._service.CreateUP(body);
            return Ok(data);
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<UserProjectModel>>> GetALLUP()
        {
            return Ok(await this._service.ListOfAllUP());
        }
        [HttpGet("{id}")]
        public async Task<ActionResult<UserProjectModel>> GetUPbyId(int id)
        {
            return Ok(await this._service.GetUPbyID(id));
        }
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateUP(int id, [FromBody] UpdateUserProjectDTO body)
        {
            var up = await this._service.UpdateUP(id, body);
            return NoContent();
        }
    }
}
