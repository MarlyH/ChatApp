using ChatApp.Domain.Models;
using ChatApp.Infrastructure.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

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

        public async Task<bool> DeleteMessageAsync(Guid messageId)
        {
            var affectedRows = await _db.ChatMessages
                .Where(m => m.Id == messageId)
                .ExecuteDeleteAsync();

            return affectedRows > 0;
        }
    }
}
