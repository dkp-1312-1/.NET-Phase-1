using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using TraineeManagement.Api.DTOs;
using TraineeManagement.Api.Repositories;

namespace TraineeManagement.Api.Services
{
    public class JWTService : IJWTService
    {
        private readonly IUserRepository _userRepository;

        public JWTService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<LoginResponseDTO> GenerateToken(LoginRequestDTO loginRequest)
        {
            var user = await _userRepository.GetUserByUsernameAsync(loginRequest.Username);
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
            var ExpiryIn = Config.ExpiryIn;

            var token = new JwtSecurityToken(
                issuer: Config.Issuer,
                audience: Config.Audience,
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(ExpiryIn),
                signingCredentials: signingCredentials);

            var tokenString = new JwtSecurityTokenHandler().WriteToken(token);
            return MapToResponse(tokenString,ExpiryIn,info);
        }
        private LoginResponseDTO MapToResponse(string tokenString,int ExpiryIn,UserInfoDTO info)
        {
            return new LoginResponseDTO
            {
                Token = tokenString,
                Expires = ExpiryIn,
                User = info
            };
        }
    }
}