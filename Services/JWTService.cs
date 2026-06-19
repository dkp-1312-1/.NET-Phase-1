using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.EntityFrameworkCore;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using TraineeManagement.Api.DTOs;
using TraineeManagement.Api.Data;
namespace TraineeManagement.Api.Services
{
    public class JWTService : IJWTService
    {
        private readonly AppDbContext _context;
        public JWTService(AppDbContext context)
        {
            _context = context;
        }
        public async Task<LoginResponseDTO> GenerateToken(LoginRequestDTO loginRequest)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Username == loginRequest.Username);
            if (user == null || !BCrypt.Net.BCrypt.EnhancedVerify(loginRequest.Password, user.PasswordHash))
            {
                return null;
            }
            UserInfoDTO info = new UserInfoDTO
            {
                Id = user.Id,
                Username = user.Username,
                Role = user.Role
            };

            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub,info.Id.ToString()),
                new Claim(JwtRegisteredClaimNames.UniqueName,info.Username),
                new Claim(ClaimTypes.Role,info.Role.ToString())
            };

            var signingCredentials = new SigningCredentials(
                Config.SecurityKey,
                SecurityAlgorithms.HmacSha256
            );
            var expiryMinutes = Config.ExpiryMinutes;

            var token = new JwtSecurityToken(
                issuer: Config.Issuer,
                audience: Config.Audience,
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(expiryMinutes),
                signingCredentials: signingCredentials);

            var tokenString = new JwtSecurityTokenHandler().WriteToken(token);
            return MapToResponse(tokenString,expiryMinutes,info);
        }
        private LoginResponseDTO MapToResponse(string tokenString,int expiryMinutes,UserInfoDTO info)
        {
            return new LoginResponseDTO
            {
                Token = tokenString,
                ExpiresInMinutes = expiryMinutes,
                User = info
            };
        }
    }
}