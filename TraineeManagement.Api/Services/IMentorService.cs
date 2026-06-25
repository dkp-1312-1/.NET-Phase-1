using System.Diagnostics.Eventing.Reader;
using TraineeManagement.Api.DTOs;
using TraineeManagement.Api.Data;
using TraineeManagement.Api.Enums;
namespace TraineeManagement.Api.Services
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
