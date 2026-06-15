using System.Diagnostics.Eventing.Reader;
using TraineeManagement1.DTOs;
using TraineeManagement1.Data;
namespace TraineeManagement1.Services
{
    public interface IMentorService
    {
        Task<PagedResponseDTO<MentorResponseDTO>> GetAll(SearchDTO trainee);
        Task<MentorResponseDTO>? GetById(int Id);
        Task<MentorResponseDTO> Create(CreateMentorRequestDTO Trainee);
        Task<MentorResponseDTO> Update(int Id, UpdateMentorRequestDTO request);
        Task<bool> Delete(int Id);
    }
}
