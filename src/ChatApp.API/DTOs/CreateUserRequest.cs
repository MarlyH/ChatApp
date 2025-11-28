using System.ComponentModel.DataAnnotations;

namespace ChatApp.API.DTOs
{
    public class CreateUserRequest
    {
        [Required]
        [MaxLength(16, ErrorMessage = "Username can only be 16 characters or less.")]
        public required string Username { get; set; }
        [Required]
        [EmailAddress]
        public required string Email { get; set; }
        [Required]
        public required string Password { get; set; }
    }
}
