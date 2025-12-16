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

        public Guid? SenderId { get; set; }  // nullable for guests
        [ForeignKey("SenderId")]
        public string? SenderName { get; set; } // optional for guests

        public string Content { get; set; } = null!;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public JsonDocument Metadata { get; set; } = JsonDocument.Parse("{}");
    }

}
