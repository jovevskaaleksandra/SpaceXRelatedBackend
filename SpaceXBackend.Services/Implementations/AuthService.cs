using Microsoft.EntityFrameworkCore;
using SpaceXBackend.DataLayer.Data;
using SpaceXBackend.DataLayer.Models;
using SpaceXBackend.Services.DTO;
using SpaceXBackend.Services.Interfaces;

namespace SpaceXBackend.Services.Implementations
{
    public class AuthService : IAuthService
    {
        private readonly SpaceXDbContext _dbContext;

        public AuthService(SpaceXDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task<AuthResponse> SignUpAsync(SignUpRequest request)
        {
            // Verify that there is no user with the e-mail from the request 
            if (await _dbContext.Users.AnyAsync(u => u.Email == request.Email))
            {
                return new AuthResponse
                {
                    Success = false,
                    Message = "User with this email already exists."
                };
            }

            // Hash the password of the user
            var passwordHash = BCrypt.Net.BCrypt.HashPassword(request.Password);

            var user = new User
            {
                FirstName = request.FirstName,
                LastName = request.LastName,
                Email = request.Email,  // plain text email -> to encrypt
                PasswordHash = passwordHash
            };

            _dbContext.Users.Add(user);
            await _dbContext.SaveChangesAsync();

            return new AuthResponse
            {
                Success = true,
                Message = "User registered successfully."
            };
        }
    }
}
