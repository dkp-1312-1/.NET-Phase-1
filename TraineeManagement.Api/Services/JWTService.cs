using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using TraineeManagement.Api.DTOs;
using TraineeManagement.Api.Models;
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
            User user = await _userRepository.GetUserByUsernameAsync(loginRequest.Username);
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

            Claim[] claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub,info.Id.ToString()),
                new Claim(JwtRegisteredClaimNames.UniqueName,info.Username),
                new Claim(ClaimTypes.Role,info.Role.ToString())
            };

            SigningCredentials signingCredentials = new SigningCredentials(
                Config.JWTSecurityKey,
                SecurityAlgorithms.HmacSha256
            );
            int expiryInSeconds = Config.JWTExpiryInSeconds;

            JwtSecurityToken token = new JwtSecurityToken(
                issuer: Config.JWTIssuer,
                audience: Config.JWTAudience,
                claims: claims,
                expires: DateTime.UtcNow.AddSeconds(expiryInSeconds),
                signingCredentials: signingCredentials);

            string tokenString = new JwtSecurityTokenHandler().WriteToken(token);
            return MapToResponse(tokenString,expiryInSeconds,info);
        }
        private LoginResponseDTO MapToResponse(string tokenString,int expiryInSeconds,UserInfoDTO info)
        {
            return new LoginResponseDTO
            {
                Token = tokenString,
                ExpiresIn = expiryInSeconds,
                User = info
            };
        }
    }
}