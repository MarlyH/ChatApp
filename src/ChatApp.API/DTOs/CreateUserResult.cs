using ChatApp.API.Models;

namespace ChatApp.API.DTOs
{
    // result passed from service -> controller
    public class CreateUserResult
    {
        public bool Succeeded { get; set; }
        public List<string> Errors { get; set; } = new();
        public AppUser? User { get; set; }
    }
}
