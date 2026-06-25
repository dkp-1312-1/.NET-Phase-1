using Microsoft.EntityFrameworkCore;
using TraineeManagement.Api.Data;
using TraineeManagement.Api.DTOs;
using TraineeManagement.Api.Enums;
 
namespace TraineeManagement.Api.Services
{
    public interface IReviewService
    {
        Task<PagedResponseDTO<ReviewResponseDTO>> GetAll(SearchDTO<RSType> search);
        Task<ReviewResponseDTO> GetById(int id);
        Task<ReviewResponseDTO> Create(CreateReviewRequestDTO request);
    }
}