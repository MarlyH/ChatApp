using ChatApp.Domain.Models;
using ChatApp.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace ChatApp.API.Repositories
{
    public class RoomMemberRepository
    {
        private readonly ChatDbContext _db;

        public RoomMemberRepository(ChatDbContext db)
        {
            _db = db;
        }

        public async Task AddRegisteredRoomMemberAsync(RoomMember roomMember)
        {
            await _db.RoomMembers.AddAsync(roomMember);
            await _db.SaveChangesAsync();
        }

        public async Task<bool> IsUserRoomMemberAsync(Guid userId, Guid roomId)
        {
            return await _db.RoomMembers.AnyAsync(rm => rm.UserId == userId && rm.RoomId == roomId);
        }
    }
}
