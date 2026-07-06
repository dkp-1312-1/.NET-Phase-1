using System.ComponentModel.DataAnnotations;
using TraineeManagement.Data.Enums;

namespace TraineeManagement.Data.DTOs
{
    public class MentorResponseDTO 
    {
        public int? Id{get;set;}
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? Email { get; set; }
        public string? Expertise { get; set; }
        public MentorStatusType? Status { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime UpdatedDate { get; set; }
    }
}
