namespace Chatroom.Models
{
    public class RoomMember
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public Guid RoomId { get; set; }
        public ChatRoom Room { get; set; } = null!;
        public Guid? UserId { get; set; } // room member can be registered user or guest.
        public string? GuestName { get; set; }
        public DateTime JoinedAt { get; set; } = DateTime.UtcNow;
    }
}
