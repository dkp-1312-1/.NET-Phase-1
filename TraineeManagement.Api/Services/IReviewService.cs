using Microsoft.EntityFrameworkCore;
using TraineeManagement.Data.Data;
using TraineeManagement.Data.DTOs;
using TraineeManagement.Data.Enums;
 
namespace TraineeManagement.Api.Services
{
    public interface IReviewService
    {
        Task<PagedResponseDTO<ReviewResponseDTO>> GetAll(SearchDTO<RSType> search);
        Task<ReviewResponseDTO> GetById(int id);
        Task<ReviewResponseDTO> Create(CreateReviewRequestDTO request);
    }
}
