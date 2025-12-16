using ChatApp.API.DTOs;
using ChatApp.API.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace ChatApp.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RoomMemberController : ControllerBase
    {
        private readonly RoomMemberService _roomMemberService;
        public RoomMemberController(RoomMemberService roomMemberService)
        {
            _roomMemberService = roomMemberService;
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> CreateRegisteredRoomMember([FromBody] JoinRoomRegisteredRequest dto)
        {
            string userIdString = User.FindFirstValue(ClaimTypes.NameIdentifier)!;
            Guid userId = Guid.Parse(userIdString);

            var result = await _roomMemberService.JoinRoomRegisteredAsync(userId, dto.Slug);
            if (!result.Succeeded)
            {
                return BadRequest(new { message = result.Message });
            }

            return Ok(new { message = result.Message });
        }
    }
}
