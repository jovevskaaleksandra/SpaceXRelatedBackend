using Microsoft.AspNetCore.Mvc;
using SpaceXBackend.Services.DTO;
using SpaceXBackend.Services.Interfaces;

namespace SpaceXBackendAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("signup")]
        public async Task<IActionResult> SignUp([FromBody] SignUpRequest request)
        {
            var response = await _authService.SignUpAsync(request);

            if (!response.Success)
                return BadRequest(response);

            return Ok(response);
        }

        [HttpPost("signin")]
        public async Task<IActionResult> SignIn([FromBody] SignInRequest request)
        {
            var response = await _authService.SignInAsync(request);

            if (!response.Success)
                return Unauthorized(response);

            return Ok(response);
        }
    }
}
