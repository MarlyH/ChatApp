using ChatApp.API.DTOs;
using ChatApp.API.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ChatApp.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly UserService _userService;

        public UserController(UserService userService)
        {
            _userService = userService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> CreateUser([FromBody] CreateUserRequest dto)
        {
            var result = await _userService.CreateNewUserAsync(dto);
            if (!result.Succeeded)
            {
                return BadRequest(new { errors = result.Errors });
            }

            return Ok(new { message = "User successfully created." });
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest dto)
        {
            var result = await _userService.LoginAsync(dto);

            if (!result.Succeeded)
            {
                return Unauthorized(new { message = result.Message });
            }

            return Ok(new { message = result.Message });
        }

        [Authorize]
        [HttpGet("me")]
        public async Task<IActionResult> GetUserProfile()
        {
            var result = await _userService.GetUserProfileAsync(User);

            if (!result.Succeeded)
            {
                return BadRequest(new { message = result.Message });
            }

            return Ok(result.Data);
        }

        [Authorize]
        [HttpGet("rooms")]
        public async Task<IActionResult> GetUserRooms()
        {
            var result = await _userService.GetUserRoomsAsync(User);

            if (!result.Succeeded)
            {
                return BadRequest(new { message = result.Message });
            }

            return Ok(result.Data);
        }


        [HttpGet("confirm-email")]
        public async Task<IActionResult> ConfirmEmail(string userId, string token)
        {
            var result = await _userService.ConfirmEmail(userId, token);
            if (!result.Succeeded)
            {
                return BadRequest(new { message = result.Message });
            }

            return Ok(new { message = result.Message });
        }

        [HttpPost("resend-confirmation")]
        public async Task<IActionResult> ResendConfirmation([FromBody] ResendConfirmationRequest dto)
        {
            var result = await _userService.ResendConfirmation(dto);
            if (!result.Succeeded)
            {
                return BadRequest(new { message = result.Message });
            }

            return Ok(new { message = result.Message });
        }

    }
}
