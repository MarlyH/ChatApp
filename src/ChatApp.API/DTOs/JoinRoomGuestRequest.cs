using System.ComponentModel.DataAnnotations;

namespace ChatApp.API.DTOs
{
    public class JoinRoomGuestRequest
    {
        [Required]
        public required string GuestName { get; set; }
    }
}
