using Microsoft.EntityFrameworkCore;
using TraineeManagement1.Data;
using TraineeManagement1.DTOs;
using TraineeManagement1.Models;
 
namespace TraineeManagement1.Services
{
    public interface ITaskAssignmentService
    {
        Task<PagedResponseDTO<TaskAssignmentResponseDTO>> GetAll(SearchDTO<TAType> search);
        Task<TaskAssignmentResponseDTO> GetById(int id);
        Task<TaskAssignmentResponseDTO> Create(CreateTaskAssignmentRequestDTO request);
        Task<TaskAssignmentResponseDTO> UpdateStatus(int id, TAType status);
    }
}