using ChatApp.API.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ChatApp.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ChatMessageController : ControllerBase
    {
        private readonly ChatMessageService _chatMsgService;

        public ChatMessageController(ChatMessageService chatMsgService)
        {
            _chatMsgService = chatMsgService;
        }
    }
}
