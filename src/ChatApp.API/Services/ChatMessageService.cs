using ChatApp.API.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ChatApp.API.Services
{
    [Route("api/[controller]")]
    [ApiController]
    public class ChatMessageService : ControllerBase
    {
        private readonly ChatMessageRepository _chatMsgRepository;
        public ChatMessageService(ChatMessageRepository chatMsgRepository)
        {
            _chatMsgRepository = chatMsgRepository;
        }
    }
}
