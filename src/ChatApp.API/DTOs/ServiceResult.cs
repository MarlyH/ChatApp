namespace ChatApp.API.DTOs
{
    /// <summary>
    /// Generic service result DTO
    /// </summary>
    public class ServiceResult
    {
        public bool Succeeded { get; set; }
        public string? Message { get; set; }
        public IEnumerable<string>? Errors { get; set; }
    }
    public class ServiceResult<T> : ServiceResult
    {
        public T? Data { get; set; }
    }
}
