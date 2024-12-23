using Microsoft.AspNetCore.Mvc;
using TASK_FLOW.NET.Auth.Attributes;
using TASK_FLOW.NET.Auth.DTO;
using TASK_FLOW.NET.Auth.Filters;
using TASK_FLOW.NET.Auth.Service.Interface;
using TASK_FLOW.NET.User.Model;

namespace TASK_FLOW.NET.Auth.Controller
{
    [Route("api/[controller]")]
    [JwtAuth]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _service;

        public AuthController(IAuthService service)
        {
            this._service = service;
        }

        /// <summary>
        /// Login User 
        /// </summary>
        /// <param name="body"></param>
        /// <returns>User Token</returns>
        /// <response code="200">Ok</response>
        /// <response code="401">Unauthorized</response>
        [AllowAnonymousAccess]
        [ServiceFilter(typeof(LocalAuthFilter))]
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult> Login([FromBody] AuthPropsDTO body)
        {
            var user = HttpContext.Items["User"] as UsersModel;
            var newToken = await this._service.GenerateToken(user);

            return StatusCode(StatusCodes.Status200OK, new { token = newToken });
        }

        /// <summary>
        /// Get Profile
        /// </summary>
        /// <returns>Return the Username of User</returns>
        /// <response code="200">Ok</response>
        /// <response code="401">Unauthorized</response>
        [HttpGet("profile")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<string>> GetProfile()
        {
            var user = await this._service.GetProfile();
            return Ok(user);
        }

        /// <summary>
        /// Log Out
        /// </summary>
        /// <returns>200</returns>
        /// <response code="200">Ok</response>
        /// <response code="401">Unauthorized</response>
        [HttpPost("logout")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult> LogOut()
        {
            await this._service.Logout();
            return Ok(new { message =" Log Out successfully"});
        }
    }
}
