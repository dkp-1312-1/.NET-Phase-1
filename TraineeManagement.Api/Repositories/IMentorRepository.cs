using TraineeManagement.Data.DTOs;
using TraineeManagement.Data.Models;
using TraineeManagement.Data.Enums;

namespace TraineeManagement.Api.Repositories
{
    public interface IMentorRepository
    {
        Task<(List<Mentor> mentors, int totalRecords)> GetMentorsAsync(SearchDTO<MentorStatusType> searchDTO);
        Task<Mentor> GetByIdAsync(int id);
        Task AddAsync(Mentor mentor);
        Task UpdateAsync(Mentor mentor);
        Task DeleteAsync(Mentor mentor);
    }
}
