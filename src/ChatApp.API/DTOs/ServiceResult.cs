namespace ChatApp.API.DTOs
{
    /// <summary>
    /// Generic service result DTO
    /// </summary>
    public class ServiceResult<T>
    {
        public bool Succeeded { get; set; }
        public string? Message { get; set; }
        public T? Data { get; set; }
        public IEnumerable<string>? Errors { get; set; }
    }
}
