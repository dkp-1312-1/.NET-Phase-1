using Microsoft.EntityFrameworkCore;
using TraineeManagement.Data.Data;
using TraineeManagement.Data.DTOs;
using TraineeManagement.Data.Enums;
 
namespace TraineeManagement.Api.Services
{
    public interface ITaskAssignmentService
    {
        Task<PagedResponseDTO<TaskAssignmentResponseDTO>> GetAll(SearchDTO<TAType> search);
        Task<TaskAssignmentResponseDTO> GetById(int id);
        Task<TaskAssignmentResponseDTO> Create(CreateTaskAssignmentRequestDTO request);
        Task<TaskAssignmentResponseDTO> UpdateStatus(int id, TAType status);
    }
}
