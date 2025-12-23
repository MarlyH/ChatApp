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

        public async Task AddRoomMemberAsync(RoomMember roomMember)
        {
            await _db.RoomMembers.AddAsync(roomMember);
            await _db.SaveChangesAsync();
        }

        public async Task<bool> IsUserRoomMemberAsync(Guid userId, Guid roomId)
        {
            return await _db.RoomMembers.AnyAsync(rm => rm.UserId == userId && rm.RoomId == roomId);
        }

        public async Task<bool> IsGuestRoomMemberAsync(string guestToken, Guid roomId)
        {
            return await _db.RoomMembers.AnyAsync(rm => rm.GuestToken == guestToken && rm.RoomId == roomId);
        }

        public async Task<RoomMember?> GetRegisteredRoomMemberAsync(Guid roomId, Guid userId)
        {
            return await _db.RoomMembers
                .FirstOrDefaultAsync(rm => rm.RoomId == roomId && rm.UserId == userId);
        }

        public async Task<RoomMember?> GetGuestRoomMemberAsync(Guid roomId, string guestToken)
        {
            return await _db.RoomMembers
                .FirstOrDefaultAsync(rm => rm.RoomId == roomId && rm.GuestToken == guestToken);
        }
    }
}
