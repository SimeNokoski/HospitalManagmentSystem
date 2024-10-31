using HospitalManagementSystem.DTO.UserDtos;
using HospitalManagementSystem.Services.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace HospitalManagementSystem.Api.Controllers
{
    [Authorize]
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
        [HttpPost("register")]
        public IActionResult Register([FromBody] RegisterUserDto registerUserDto)
        {
                _userService.Register(registerUserDto);
                return StatusCode(StatusCodes.Status201Created, "User was created");
        }

        [AllowAnonymous]
        [HttpPost("login")]
        public IActionResult LoginUser([FromBody] LoginUserDto loginDto)
        {
                string token = _userService.LoginUser(loginDto);
                return Ok(token);   
        }
    }
}
