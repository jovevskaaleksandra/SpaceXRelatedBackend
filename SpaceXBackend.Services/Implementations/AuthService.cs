using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using SpaceXBackend.DataLayer.Data;
using SpaceXBackend.DataLayer.Models;
using SpaceXBackend.Services.DTO;
using SpaceXBackend.Services.Interfaces;
using System.Security.Claims;
using System.Text;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;

namespace SpaceXBackend.Services.Implementations
{
    public class AuthService : IAuthService
    {
        private readonly SpaceXDbContext _dbContext;
        private readonly IEncryptionService _encryptionService;
        private readonly IConfiguration _config;

        public AuthService(SpaceXDbContext dbContext, IEncryptionService encryptionService, IConfiguration config)
        {
            _dbContext = dbContext;
            _encryptionService = encryptionService;
            _config = config;
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

            var token = GenerateJwtToken(user);

            return new AuthDto
            {
                Success = true,
                Message = "Login successful.",
                Token = token
            };
        }

        private string GenerateJwtToken(User user)
        {
            var jwtConfig = _config.GetSection("Jwt");
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtConfig["Key"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
                new Claim(JwtRegisteredClaimNames.Email, user.Email)
            };

            var token = new JwtSecurityToken(
                issuer: jwtConfig["Issuer"],
                audience: jwtConfig["Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddHours(2),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
