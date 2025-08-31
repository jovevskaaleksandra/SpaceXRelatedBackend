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
        private readonly IEncryptionService _encryptionService;

        public AuthService(SpaceXDbContext dbContext, IEncryptionService encryptionService)
        {
            _dbContext = dbContext;
            _encryptionService = encryptionService;
        }

        public async Task<AuthDto> SignUpAsync(SignUpRequest request)
        {
            // Encrypt the email of the user
            var encryptedEmail = _encryptionService.Encrypt(request.Email);
            // Verify that there is no user with the e-mail from the request 
            if (await _dbContext.Users.AnyAsync(u => u.Email == encryptedEmail))
            {
                return new AuthDto
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
                Email = encryptedEmail,  
                PasswordHash = passwordHash
            };

            _dbContext.Users.Add(user);
            await _dbContext.SaveChangesAsync();

            return new AuthDto
            {
                Success = true,
                Message = "User registered successfully."
            };
        }

        public async Task<AuthDto> SignInAsync(SignInRequest request)
        {
            var encryptedEmail = _encryptionService.Encrypt(request.Email);

            var user = await _dbContext.Users.FirstOrDefaultAsync(u => u.Email == encryptedEmail);

            if (user == null)
            {
                return new AuthDto
                {
                    Success = false,
                    Message = "Invalid email."
                };
            }

            bool validPassword = BCrypt.Net.BCrypt.Verify(request.Password, user.PasswordHash);

            if (!validPassword)
            {
                return new AuthDto
                {
                    Success = false,
                    Message = "Invalid password."
                };
            }

            return new AuthDto
            {
                Success = true,
                Message = "Login successful."
            };
        }
    }
}
