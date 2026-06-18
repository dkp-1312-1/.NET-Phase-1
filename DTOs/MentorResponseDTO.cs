using System.ComponentModel.DataAnnotations;
using TraineeManagement1.Models;

namespace TraineeManagement1.DTOs
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
