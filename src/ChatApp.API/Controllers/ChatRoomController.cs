using ChatApp.API.DTOs;
using ChatApp.API.Services;
using ChatApp.Domain.Models;
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
            ChatRoom newChatRoom;
            try
            {
                newChatRoom = await _chatRoomService.CreateRoom(dto);
            }
            catch (InvalidOperationException ex)
            {
                return Conflict(new { success = false, message = ex.Message });
            }
            catch
            {
                throw; // to global handler.
            }
            
            return Ok(new { success = true, message = "Chatroom successfully created.", roomId = newChatRoom.Id });
        }
    }
}
