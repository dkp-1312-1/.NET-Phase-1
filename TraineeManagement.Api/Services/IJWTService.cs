using System.Security.Claims;
using TraineeManagement.Api.DTOs;
namespace TraineeManagement.Api.Services
{
    public interface IJWTService
    {
        Task<LoginResponseDTO> GenerateToken(LoginRequestDTO info);
    }
}
