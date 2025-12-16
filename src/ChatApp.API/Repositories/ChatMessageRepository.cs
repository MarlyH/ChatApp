using ChatApp.Infrastructure.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ChatApp.API.Repositories
{
    [Route("api/[controller]")]
    [ApiController]
    public class ChatMessageRepository : ControllerBase
    {
        private readonly ChatDbContext _db;
        public ChatMessageRepository(ChatDbContext db)
        {
            _db = db;
        }
    }
}
