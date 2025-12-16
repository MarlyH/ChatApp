using ChatApp.API.Models;
using System.ComponentModel.DataAnnotations.Schema;

namespace ChatApp.Domain.Models
{
    public class RoomMember
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public Guid RoomId { get; set; }
        [ForeignKey("RoomId")]
        public ChatRoom Room { get; set; } = null!;

        // registered user
        public Guid? UserId { get; set; }
        [ForeignKey("UserId")]
        public AppUser? User { get; set; }

        // guest user
        public string? GuestToken { get; set; }
        public string? GuestName { get; set; }

        public DateTime JoinedAt { get; set; } = DateTime.UtcNow;
    }
}
