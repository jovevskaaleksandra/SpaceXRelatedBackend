namespace SpaceXBackend.Services.DTO
{
    public class AuthDto
    {
        public bool Success { get; set; }
        public string Message { get; set; } = string.Empty;
        public string? Token { get; set; }
    }
}
