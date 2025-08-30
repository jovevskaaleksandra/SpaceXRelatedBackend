using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SpaceXBackend.Services.Interfaces;

namespace SpaceXBackendAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SpaceXController : ControllerBase
    {
        private readonly ISpaceXService _spaceXservice;

        public SpaceXController(ISpaceXService spaceXservice)
        {
            _spaceXservice = spaceXservice;
        }

        [HttpGet("latest")]
        public async Task<IActionResult> Latest()
        {
            var launch = await _spaceXservice.GetLatestLaunchAsync();
            if (launch == null)
                return NotFound();

            return Ok(launch);
        }

        [HttpGet("upcoming")]
        public async Task<IActionResult> Upcoming()
        {
            var launches = await _spaceXservice.GetUpcomingLaunchesAsync();
            if (launches.Count == 0)
                return NotFound();

            return Ok(launches);
        }

        [HttpGet("past")]
        public async Task<IActionResult> Past()
        {
            var launches = await _spaceXservice.GetPastLaunchesAsync();
            if (launches.Count == 0)
                return NotFound();

            return Ok(launches);
        }
    }
}
