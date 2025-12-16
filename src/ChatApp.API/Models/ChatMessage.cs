using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json;

namespace ChatApp.Domain.Models
{
    public class ChatMessage
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public Guid RoomId { get; set; }
        [ForeignKey("RoomId")]
        public ChatRoom Room { get; set; } = null!;

        public Guid SenderId { get; set; }
        [ForeignKey("SenderId")]
        public RoomMember? Sender { get; set; }

        public string Content { get; set; } = null!;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public JsonDocument Metadata { get; set; } = JsonDocument.Parse("{}");
    }

}
