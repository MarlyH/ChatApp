using System.ComponentModel.DataAnnotations;

namespace ChatApp.API.DTOs
{
    public class CreateRoomRequest
    {
        [Required]
        [MaxLength(32, ErrorMessage = "Room name can only be 32 characters or less.")]
        public required string Name { get; set; }
        public bool IsPrivate { get; set; } = false;
    }
}
