using Microsoft.EntityFrameworkCore;
using TraineeManagement1.Data;
using TraineeManagement1.DTOs;
using TraineeManagement1.Models;
 
namespace TraineeManagement1.Services
{
    public interface IReviewService
    {
        Task<PagedResponseDTO<ReviewResponseDTO>> GetAll(SearchDTO<RSType> search);
        Task<ReviewResponseDTO> GetById(int id);
        Task<ReviewResponseDTO> Create(CreateReviewRequestDTO request);
    }
}