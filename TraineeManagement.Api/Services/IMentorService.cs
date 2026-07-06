using System.Diagnostics.Eventing.Reader;
using TraineeManagement.Data.DTOs;
using TraineeManagement.Data.Data;
using TraineeManagement.Data.Enums;
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
