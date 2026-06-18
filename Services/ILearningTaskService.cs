using TraineeManagement1.DTOs;
using TraineeManagement1.Models;

namespace TraineeManagement1.Services
{
    public interface ILearningTaskService
    {
         Task<PagedResponseDTO<LearningTaskResponseDTO>> GetAll(SearchDTO<LTStatusType> searchDTO);
        Task<LearningTaskResponseDTO> GetById(int id);
        Task<LearningTaskResponseDTO> Create(CreateLearningTaskRequestDTO request);
        Task<LearningTaskResponseDTO> Update(int id, UpdateLearningTaskRequestDTO request);
        Task<bool> Delete(int id);
    }
}