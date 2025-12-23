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
    [Route("api/rooms/{roomSlug}")]
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
        [HttpPost("join")]
        public async Task<IActionResult> CreateRegisteredRoomMember(string roomSlug)
        {
            var user = await _userManager.FindByIdAsync(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
            if (user is null) 
            { 
                return Unauthorized(); 
            }

            var result = await _roomMemberService.JoinRoomRegisteredAsync(user, roomSlug);
            if (!result.Succeeded)
            {
                return BadRequest(new { message = result.Message });
            }

            return Ok(new { message = result.Message });
        }

        [HttpPost("join-guest")]
        public async Task<IActionResult> CreateGuestRoomMember(string roomSlug, [FromBody] JoinRoomGuestRequest dto)
        {
            var result = await _roomMemberService.JoinRoomGuestAsync(dto.GuestName, roomSlug);
            if (!result.Succeeded)
            {
                return BadRequest(new { message = result.Message });
            }
            string guestToken = result.Data!;

            return Ok(new { message = result.Message, guestToken });
        }
    }
}
