using TraineeManagement.Data.DTOs;
using TraineeManagement.Data.Enums;

namespace TraineeManagement.Api.Services
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
