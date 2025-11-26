namespace ChatApp.Domain.Models
{
    public class ChatRoom
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string Name { get; set; } = null!;
        public string Slug { get; set; } = null!;
        public bool IsInviteOnly { get; set; } = false;

        public ICollection<ChatMessage> Messages { get; set; } = new List<ChatMessage>();
        public ICollection<RoomMember> Members { get; set; } = new List<RoomMember>();
    }
}
