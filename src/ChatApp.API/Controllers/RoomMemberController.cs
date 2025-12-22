using ChatApp.API.DTOs;
using ChatApp.API.Models;
using ChatApp.API.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace ChatApp.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RoomMemberController : ControllerBase
    {
        private readonly RoomMemberService _roomMemberService;
        private readonly UserManager<AppUser> _userManager;
        public RoomMemberController(RoomMemberService roomMemberService, UserManager<AppUser> userManager)
        {
            _roomMemberService = roomMemberService;
            _userManager = userManager;
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> CreateRegisteredRoomMember([FromBody] JoinRoomRegisteredRequest dto)
        {
            var user = await _userManager.FindByIdAsync(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
            if (user is null) 
            { 
                return Unauthorized(); 
            }

            var result = await _roomMemberService.JoinRoomRegisteredAsync(user, dto.Slug);
            if (!result.Succeeded)
            {
                return BadRequest(new { message = result.Message });
            }

            return Ok(new { message = result.Message });
        }
    }
}
