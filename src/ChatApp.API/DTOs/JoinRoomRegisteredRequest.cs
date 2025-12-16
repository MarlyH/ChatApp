using System.ComponentModel.DataAnnotations;

namespace ChatApp.API.DTOs
{
    public class JoinRoomRegisteredRequest
    {
        [Required]
        public required string Slug { get; set; }
    }
}
