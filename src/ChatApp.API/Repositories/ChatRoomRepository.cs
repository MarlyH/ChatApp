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
    }

}
