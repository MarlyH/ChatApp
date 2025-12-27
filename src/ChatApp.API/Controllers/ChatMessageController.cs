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

        public ChatMessageController(ChatMessageService chatMsgService)
        {
            _chatMsgService = chatMsgService;
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

        [HttpGet]
        public async Task<IActionResult> GetAllRoomMessages(string roomSlug)
        {
            var result = await _chatMsgService.GetAllRoomMessagesAsync(roomSlug);
            if (!result.Succeeded)
            {
                return NotFound(new { message = result.Message });
            }

            return Ok(result.Data);
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteMessage(string roomSlug, Guid messageId)
        {
            var result = await _chatMsgService.DeleteMessageAsync(messageId, roomSlug);
            if (!result.Succeeded)
            {
                return NotFound(new { message = result.Message });
            }

            return Ok();
        }
    }
}
