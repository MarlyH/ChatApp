using ChatApp.API.DTOs;
using ChatApp.API.Services;
using Microsoft.AspNetCore.Http;
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
            try
            {
                await _chatRoomService.CreateRoom(dto);
            }
            catch (InvalidOperationException ex)
            {
                return Conflict(new { message = ex.Message });
            }
            catch
            {
                throw; // to global handler.
            }
            
            return Ok("Room successfully craeted");
        }
    }
}
