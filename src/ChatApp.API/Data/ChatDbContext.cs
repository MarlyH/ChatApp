using ChatApp.API.Models;
using ChatApp.Domain.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace ChatApp.Infrastructure.Data
{
    public class ChatDbContext : IdentityDbContext<AppUser, IdentityRole<Guid>, Guid>
    {
        public ChatDbContext(DbContextOptions<ChatDbContext> options) : base(options) { }

        public DbSet<ChatRoom> ChatRooms { get; set; } = null!;
        public DbSet<ChatMessage> ChatMessages { get; set; } = null!;
        public DbSet<RoomMember> RoomMembers { get; set; } = null!;
    }
}
