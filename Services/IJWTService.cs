using System.Security.Claims;
using TraineeManagement1.DTOs;
namespace TraineeManagement1.Services
{
    public interface IJWTService
    {
        Task<LoginResponseDTO> GenerateToken(LoginRequestDTO info);
    }
}
