using System.ComponentModel.DataAnnotations;

namespace ChatApp.API.DTOs
{
    public class ResendConfirmationRequest
    {
        [EmailAddress]
        [Required]
        public required string Email { get; set; }
    }
}
