namespace ChatApp.API.DTOs
{
    public class GetRoomsResponse
    {
        public Guid Id { get; init; }
        public string Name { get; init; } = null!;
        public string Slug { get; init; } = null!;
    }
}
