namespace ChatApp.API.DTOs
{
    public class LoginRequest
    {
        public required string Username { get; set; }
        public required string Password { get; set; }
        public bool IsPersistent { get; set; }
    }
}
