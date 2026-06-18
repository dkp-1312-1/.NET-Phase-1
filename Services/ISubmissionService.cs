using Microsoft.EntityFrameworkCore;
using TraineeManagement1.Data;
using TraineeManagement1.DTOs;
using TraineeManagement1.Models;
 
namespace TraineeManagement1.Services
{
    public interface ISubmissionService
    {
        Task<PagedResponseDTO<SubmissionResponseDTO>> GetAll(SearchDTO<SubType> search);
        Task<SubmissionResponseDTO> GetById(int id);
        Task<SubmissionResponseDTO> Create(CreateSubmissionRequestDTO request);
    }
}