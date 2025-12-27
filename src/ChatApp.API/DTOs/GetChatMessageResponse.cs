namespace ChatApp.API.DTOs
{
    public class GetChatMessageResponse
    {
        public Guid Id { get; set; }
        public string Content { get; set; } = default!;
        public DateTime CreatedAt { get; set; }
        public string SenderUsername { get; set; } = default!;
    }
}
