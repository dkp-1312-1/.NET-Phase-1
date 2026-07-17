using TraineeManagement.Data.DTOs;
using TraineeManagement.Data.Models;
using TraineeManagement.Data.Enums;
using TraineeManagement.Api.Repositories;
using System.Linq;

namespace TraineeManagement.Api.Services
{
    public class MentorService : IMentorService
    {
        private readonly IMentorRepository _mentorRepository;

        public MentorService(IMentorRepository mentorRepository)
        {
            _mentorRepository = mentorRepository;
        }

        public async Task<PagedResponseDTO<MentorResponseDTO>> GetAll(SearchDTO<MentorStatusType> mentor)
        {
            (List<Mentor>? mentors, int totalRecords) = await _mentorRepository.GetMentorsAsync(mentor);

            return new PagedResponseDTO<MentorResponseDTO>
            {
                PageNumber = mentor.PageNumber,
                PageSize = mentor.PageSize,
                TotalRecords = totalRecords,
                Data = mentors.Select(MapToResponse)
            };
        }

        public async Task<MentorResponseDTO> GetById(int Id)
        {
            Mentor mentor = await _mentorRepository.GetByIdAsync(Id);
            return mentor != null ? MapToResponse(mentor) : null!;
        }

        public async Task<MentorResponseDTO> Create(CreateMentorRequestDTO mentor)
        {
            Mentor newMentor = new Mentor(mentor);

            await _mentorRepository.AddAsync(newMentor);
            return MapToResponse(newMentor);
        }

        public async Task<MentorResponseDTO> Update(int Id, UpdateMentorRequestDTO mentor)
        {
            Mentor updatedMentor = await _mentorRepository.GetByIdAsync(Id);
            if (updatedMentor == null)
                return null!;

            updatedMentor.FirstName = mentor.FirstName;
            updatedMentor.LastName = mentor.LastName;
            updatedMentor.Email = mentor.Email;
            updatedMentor.Expertise = mentor.Expertise;
            updatedMentor.Status = mentor.Status;
            updatedMentor.UpdatedDate = DateTime.UtcNow;

            await _mentorRepository.UpdateAsync(updatedMentor);
            return MapToResponse(updatedMentor);
        }

        public async Task<bool> Delete(int Id)
        {
            Mentor mentor = await _mentorRepository.GetByIdAsync(Id);
            if (mentor == null)
                return false;

            await _mentorRepository.DeleteAsync(mentor);
            return true;
        }

        private MentorResponseDTO MapToResponse(Mentor mentor)
        {
            return new MentorResponseDTO
            {
                Id = mentor.Id,
                FirstName = mentor.FirstName,
                LastName = mentor.LastName,
                Email = mentor.Email,
                Expertise = mentor.Expertise,
                Status = mentor.Status,
                CreatedDate = mentor.CreatedDate,
                UpdatedDate = mentor.UpdatedDate
            };
        }
    }
}
