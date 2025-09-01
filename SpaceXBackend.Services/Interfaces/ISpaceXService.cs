using SpaceXBackend.Services.DTO;

namespace SpaceXBackend.Services.Interfaces
{
    public interface ISpaceXService
    {
        Task<SpaceXLaunchDto?> GetLatestLaunchAsync();
        Task<List<SpaceXLaunchDto>> GetUpcomingLaunchesAsync();
        Task<List<SpaceXLaunchDto>> GetPastLaunchesAsync();
    }
}
