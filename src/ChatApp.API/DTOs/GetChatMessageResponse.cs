namespace ChatApp.API.DTOs
{
    public class GetChatMessageResponse
    {
        public string Content { get; set; } = default!;
        public DateTime CreatedAt { get; set; }
        public string SenderUsername { get; set; } = default!;
    }
}
