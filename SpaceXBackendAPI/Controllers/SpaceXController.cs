using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SpaceXBackend.Services.Interfaces;

namespace SpaceXBackendAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SpaceXController : ControllerBase
    {
        private readonly ISpaceXService _spaceX;

        public SpaceXController(ISpaceXService spaceX)
        {
            _spaceX = spaceX;
        }

        [HttpGet("latest")]
        public async Task<IActionResult> Latest()
        {
            try
            {
                var json = await _spaceX.GetLatestLaunchAsync();
                return Content(json, "application/json");
            }
            catch (HttpRequestException ex)
            {
                return StatusCode(502, new { error = ex.Message });
            }
        }

        [HttpGet("upcoming")]
        public async Task<IActionResult> Upcoming()
        {
            try
            {
                var json = await _spaceX.GetUpcomingLaunchesAsync();
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
                var json = await _spaceX.GetPastLaunchesAsync();
                return Content(json, "application/json");
            }
            catch (HttpRequestException ex)
            {
                return StatusCode(502, new { error = ex.Message });
            }
        }
    }
}
