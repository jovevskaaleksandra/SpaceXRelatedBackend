namespace SpaceXBackend.Services.DTO
{
    public class SpaceXLaunchDto
    {
        public string Id { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public DateTime DateUtc { get; set; }
        public bool? Success { get; set; }
        public string? Details { get; set; }
        public string? PatchImage { get; set; }
        public string? Webcast { get; set; }
        public string? Wikipedia { get; set; }
    }
}
