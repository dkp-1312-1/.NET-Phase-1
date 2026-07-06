using Microsoft.EntityFrameworkCore;
using TraineeManagement.Data.Data;
using TraineeManagement.Data.DTOs;
using TraineeManagement.Data.Enums;
 
namespace TraineeManagement.Api.Services
{
    public interface ISubmissionService
    {
        Task<PagedResponseDTO<SubmissionResponseDTO>> GetAll(SearchDTO<SubType> search);
        Task<SubmissionResponseDTO> GetById(int id);
        Task<SubmissionResponseDTO> Create(CreateSubmissionRequestDTO request);
    }
}
