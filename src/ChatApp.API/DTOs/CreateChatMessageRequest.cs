using System.ComponentModel.DataAnnotations;

namespace ChatApp.API.DTOs
{
    public class CreateChatMessageRequest
    {
        [Required]
        [MinLength(1)]
        [MaxLength(255)]
        public required string Content { get; set; }
        public string? GuestToken { get; set; }
    }
}
