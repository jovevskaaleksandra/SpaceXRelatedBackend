using SpaceXBackend.Services.DTO;

namespace SpaceXBackend.Services.Interfaces
{
    public interface IAuthService
    {
        Task<AuthDto> SignUpAsync (SignUpRequest request);

        Task<AuthDto> SignInAsync (SignInRequest request);
    }
}
