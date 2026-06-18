using System.Diagnostics.Eventing.Reader;
using TraineeManagement1.DTOs;
using TraineeManagement1.Data;
using TraineeManagement1.Models;
namespace TraineeManagement1.Services
{
    public interface IMentorService
    {
        Task<PagedResponseDTO<MentorResponseDTO>> GetAll(SearchDTO<MentorStatusType> mentor);
        Task<MentorResponseDTO>? GetById(int Id);
        Task<MentorResponseDTO> Create(CreateMentorRequestDTO mentor);
        Task<MentorResponseDTO> Update(int Id, UpdateMentorRequestDTO request);
        Task<bool> Delete(int Id);
    }
}
