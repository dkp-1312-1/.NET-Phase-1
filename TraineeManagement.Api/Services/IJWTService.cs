using System.Security.Claims;
using TraineeManagement.Data.DTOs;
namespace TraineeManagement.Api.Services
{
    public interface IJWTService
    {
        Task<LoginResponseDTO> GenerateToken(LoginRequestDTO info);
    }
}
