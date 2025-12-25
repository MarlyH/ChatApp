using ChatApp.API.Models;
using ChatApp.Domain.Models;
using ChatApp.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System;

namespace ChatApp.API.Repositories
{
    public class ChatRoomRepository
    {
        private readonly ChatDbContext _db;

        public ChatRoomRepository(ChatDbContext db)
        {
            _db = db;
        }

        public async Task<bool> SlugExistsAsync(string slug)
        {
            return await _db.ChatRooms.AnyAsync(r => r.Slug == slug);
        }

        public async Task AddRoomAsync(ChatRoom room)
        {
            await _db.ChatRooms.AddAsync(room);
            await _db.SaveChangesAsync();
        }

        public async Task<List<ChatRoom>> GetPublicRoomsAsync()
        {
            return await _db.ChatRooms.Where(r => !r.IsInviteOnly).ToListAsync();
        }

        public async Task<ChatRoom?> GetRoomBySlugAsync(string slug)
        {
            return await _db.ChatRooms.FirstOrDefaultAsync(r => r.Slug == slug);
        }

        public async Task<List<ChatRoom>> GetRoomsByUserAsync(AppUser user)
        {
            return await _db.RoomMembers
                .Where(rm => rm.UserId == user.Id)
                .Select(rm => rm.Room)
                .ToListAsync();
        }

        public async Task<ChatRoom?> GetRoomWithMessagesBySlugAsync(string slug)
        {
            return await _db.ChatRooms
                .Include(r => r.Messages)
                .ThenInclude(m => m.Sender)
                .FirstOrDefaultAsync(r => r.Slug == slug);
        }
    }
}
