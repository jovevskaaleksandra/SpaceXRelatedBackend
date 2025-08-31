using SpaceXBackend.Services.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpaceXBackend.Services.Interfaces
{
    public interface IAuthService
    {
        Task<AuthDto> SignUpAsync (SignUpRequest request);

        Task<AuthDto> SignInAsync (SignInRequest request);
    }
}
