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
        public Guid? UserId { get; set; } // room member can be registered user or guest.
        [ForeignKey("UserId")]
        public AppUser? User { get; set; }
        public string? GuestName { get; set; }
        public DateTime JoinedAt { get; set; } = DateTime.UtcNow;
    }
}
