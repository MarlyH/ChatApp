using ChatApp.Domain.Models;
using ChatApp.Infrastructure.Data;
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

        public async Task CreateMessageAsync(ChatMessage message)
        {
            await _db.ChatMessages.AddAsync(message);
            await _db.SaveChangesAsync();
        }
    }
}
