using ChatApp.Domain.Models;

namespace ChatApp.API.DTOs
{
    public class CreateRoomResult
    {
        public bool Successful { get; set; }
        public string? Message { get; set; }
        public ChatRoom? Chatroom { get; set; }
    }
}
