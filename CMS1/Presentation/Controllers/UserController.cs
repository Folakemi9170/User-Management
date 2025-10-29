using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using UserManagement.Application.DTO.UserDto;
using UserManagement.Application.DTO.UserRoleDto;
using UserManagement.Application.Interfaces;
using UserManagement.Application.Services;

namespace UserManagement.Presentation.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [AllowAnonymous]
        [HttpPost("signup")]
        public async Task<ActionResult> RegisterUser([FromBody] CreateUserDto dto)
        { 
            try
            {
                var userResponse = await _userService.CreateUser(dto);
                return Ok(new
                {
                    email = userResponse.Email,
                    message = "User Registration Successful."
                });
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [AllowAnonymous]
        [HttpPost("login")]
        public async Task<ActionResult> Login([FromBody] UserLoginDto loginModel)
        {
            try
            {
                var loginResponse = await _userService.Login(loginModel);
                return Ok(loginResponse);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [Authorize]
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateUser(int id, [FromBody] UpdateUserDto dto)
        {
            if (dto == null)
                return BadRequest("Invalid request data.");

            var result = await _userService.UpdateUser(id, dto);
            return Ok(new
            {
                message = "User updated successfully",
                data = result
            });
        }

        //[HttpGet]
        //public async Task<ActionResult<List<RoleResponseDto>>> GetAllUsers()
        //{
        //    var roles = await _userService.GetAllUsers();
        //    return Ok(roles);
        //}
    }
}
