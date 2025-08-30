using SpaceXBackend.Services.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpaceXBackend.Services.Interfaces
{
    public interface ISpaceXService
    {
        Task<SpaceXLaunchDto?> GetLatestLaunchAsync();
        Task<List<SpaceXLaunchDto>> GetUpcomingLaunchesAsync();
        Task<List<SpaceXLaunchDto>> GetPastLaunchesAsync();
    }
}
