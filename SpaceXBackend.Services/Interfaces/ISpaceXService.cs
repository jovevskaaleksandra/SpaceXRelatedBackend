using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpaceXBackend.Services.Interfaces
{
    public interface ISpaceXService
    {
        Task<string> GetLatestLaunchAsync();
        Task<string> GetUpcomingLaunchesAsync();
        Task<string> GetPastLaunchesAsync();
    }
}
