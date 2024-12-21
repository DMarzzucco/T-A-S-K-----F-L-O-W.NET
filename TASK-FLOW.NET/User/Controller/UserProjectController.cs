using Microsoft.AspNetCore.Mvc;
using TASK_FLOW.NET.User.DTO;
using TASK_FLOW.NET.User.Service.Interface;

namespace TASK_FLOW.NET.User.Controller
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserProjectController : ControllerBase
    {
        private readonly IUserService _service;

        public UserProjectController(IUserService service)
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
        public async Task<ActionResult> RegisterUserAndProject(UserProjectDTO body)
        {
            var data = await this._service.RelationProject(body);
            return Ok(data);
        }
    }
}
