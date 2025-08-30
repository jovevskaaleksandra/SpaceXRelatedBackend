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
            try
            {
                var json = await _spaceXservice.GetUpcomingLaunchesAsync();
                return Content(json, "application/json");
            }
            catch (HttpRequestException ex)
            {
                return StatusCode(502, new { error = ex.Message });
            }
        }

        [HttpGet("past")]
        public async Task<IActionResult> Past()
        {
            try
            {
                var json = await _spaceXservice.GetPastLaunchesAsync();
                return Content(json, "application/json");
            }
            catch (HttpRequestException ex)
            {
                return StatusCode(502, new { error = ex.Message });
            }
        }
    }
}
