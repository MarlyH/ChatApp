using ChatApp.API.DTOs;
using ChatApp.API.Services;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace ChatApp.API.Controllers
{
    [Route("api/rooms/{roomSlug}/messages")]
    [ApiController]
    public class ChatMessageController : ControllerBase
    {
        private readonly ChatMessageService _chatMsgService;
        private readonly RoomMemberService _roomMemberService;
        private readonly ChatRoomService _chatRoomService;

        public ChatMessageController(
            ChatMessageService chatMsgService, 
            RoomMemberService roomMemberService, 
            ChatRoomService chatRoomService)
        {
            _chatMsgService = chatMsgService;
            _roomMemberService = roomMemberService;
            _chatRoomService = chatRoomService;
        }

        [HttpPost]
        public async Task<IActionResult> CreateMessage(string roomSlug, [FromBody] CreateChatMessageRequest messageDto)
        {
            Guid? userId = null;
            if (User.Identity!.IsAuthenticated)
            {
                userId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
            }

            var result = await _chatMsgService.SendMessageAsync(roomSlug, messageDto.Content, userId, messageDto.GuestToken);

            if (!result.Succeeded)
            {
                return BadRequest(new { message =  result.Message });
            }

            return Ok(new { message = result.Message });
        }
    }
}
