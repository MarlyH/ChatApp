using ChatApp.API.DTOs;
using ChatApp.API.Services;
using Microsoft.AspNetCore.Mvc;

namespace ChatApp.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ChatRoomController : ControllerBase
    {
        private readonly ChatRoomService _chatRoomService;

        public ChatRoomController(ChatRoomService chatRoomService)
        {
            _chatRoomService = chatRoomService;
        }

        [HttpPost]
        public async Task<IActionResult> CreateRoom([FromBody] CreateRoomRequest dto)
        {
            var result = await _chatRoomService.CreateRoom(dto);

            if (!result.Successful)
            {
                return BadRequest(new { message = result.Message });
            }

            return Ok(new { message = result.Message, roomId = result.Chatroom?.Id });
        }
    }
}
