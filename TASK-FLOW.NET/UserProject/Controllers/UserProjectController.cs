using Microsoft.AspNetCore.Mvc;
using TASK_FLOW.NET.UserProject.DTO;
using TASK_FLOW.NET.UserProject.Model;
using TASK_FLOW.NET.UserProject.Services.Interface;

namespace TASK_FLOW.NET.UserProject.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserProjectController : ControllerBase
    {
        private readonly IUserProjectService _service;

        public UserProjectController(IUserProjectService service)
        {
            _service = service;
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
            var data = await _service.CreateUP(body);
            return Ok(data);
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<UserProjectModel>>> GetALLUP()
        {
            return Ok(await _service.ListOfAllUP());
        }
        [HttpGet("{id}")]
        public async Task<ActionResult<UserProjectModel>> GetUPbyId(int id)
        {
            return Ok(await _service.GetUPbyID(id));
        }
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateUP(int id, [FromBody] UpdateUserProjectDTO body)
        {
            var up = await _service.UpdateUP(id, body);
            return NoContent();
        }
    }
}
