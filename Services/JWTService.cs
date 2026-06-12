using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.EntityFrameworkCore;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using TraineeManagement1.DTOs;
using TraineeManagement1.Data;
namespace TraineeManagement1.Services
{
    public class JWTService : IJWTService
    {
        private readonly AppDbContext _context;
        private readonly IConfiguration _configuration;
        public JWTService(AppDbContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
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
            var jwtSettings = _configuration.GetSection("jwt");
            var key = Encoding.UTF8.GetBytes(jwtSettings["key"]);

            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub,info.Id.ToString()),
                new Claim(JwtRegisteredClaimNames.UniqueName,info.Username),
                new Claim(ClaimTypes.Role,info.Role.ToString())
            };

            var signingCredentials = new SigningCredentials(
                new SymmetricSecurityKey(key),
                SecurityAlgorithms.HmacSha256
            );
            var expiryMinutes = Convert.ToInt32(jwtSettings["ExpiryMinutes"]);

            var token = new JwtSecurityToken(
                issuer: jwtSettings["Issuer"],
                audience: jwtSettings["Audience"],
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
                ExpiresIn = expiryMinutes,
                User = info
            };
        }
    }
}