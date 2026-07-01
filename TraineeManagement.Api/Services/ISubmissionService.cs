using Microsoft.EntityFrameworkCore;
using TraineeManagement.Api.Data;
using TraineeManagement.Api.DTOs;
using TraineeManagement.Api.Enums;
 
namespace TraineeManagement.Api.Services
{
    public interface ISubmissionService
    {
        Task<PagedResponseDTO<SubmissionResponseDTO>> GetAll(SearchDTO<SubType> search);
        Task<SubmissionResponseDTO> GetById(int id);
        Task<SubmissionResponseDTO> Create(CreateSubmissionRequestDTO request);
    }
}