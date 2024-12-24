using Microsoft.AspNetCore.Mvc;
using TASK_FLOW.NET.Auth.Attributes;
using TASK_FLOW.NET.User.DTO;
using TASK_FLOW.NET.User.Enums;
using TASK_FLOW.NET.User.Model;
using TASK_FLOW.NET.User.Service.Interface;

namespace TASK_FLOW.NET.User.Controller
{
    [Route("api/[controller]")]
    [JwtAuth]
    [AuthRoles]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _service;

        public UserController(IUserService service)
        {
            this._service = service;
        }

        /// <summary>
        /// Register a new User
        /// </summary>
        /// <param name="body"></param>
        /// <returns>A new User registered</returns>
        /// <response code="201">User Registered</response>
        /// <response code="409">Conflict between repeat dates</response>
        [AllowAnonymousAccess]
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        public async Task<ActionResult<UsersModel>> RegisterUser([FromBody] CreateUserDTO body)
        {
            var user = await this._service.CreateUser(body);
            return CreatedAtAction(nameof(GetAllUser), new { id = user.Id }, user);
        }

        /// <summary>
        /// Get All User
        /// </summary>
        /// <returns>A List of All Users</returns>
        /// <response code="200">List of User</response>
        /// <response code="400">Bad Request</response>
        [Roles(ROLES.ADMIN)]
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<IEnumerable<UsersModel>>> GetAllUser()
        {
            return Ok(await this._service.GetAll());
        }

        /// <summary>
        /// Get User by Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Get a user according his Id</returns>
        /// <response code="200">User</response>
        /// <response code="404">User not Found</response>
        [Roles(ROLES.ADMIN)]
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<UsersModel>> GetUserById(int id)
        {
            var user = await this._service.GetById(id);
            return Ok(user);
        }

        /// <summary>
        /// Update User 
        /// </summary>
        /// <param name="id"></param>
        /// <param name="body"></param>
        /// <returns>Update a user according his id </returns>
        /// <response code="204">User Updated</response>
        /// <response code="404">User not found</response>
        [Roles(ROLES.BASIC)]
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> UpdateUser(int id, [FromBody] UpdateUserDTO body)
        {
            await this._service.UpdateUser(id, body);
            return NoContent();
        }

        /// <summary>
        /// Delte User
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Delete a User according his Id</returns>
        /// <response code="204">User Deleted</response>
        /// <response code="404">User Not Found</response>
        [Roles(ROLES.BASIC)]
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DelteUser(int id)
        {
            await this._service.DeleteUser(id);
            return NoContent();
        }
    }
}
