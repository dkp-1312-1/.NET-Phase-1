using System.ComponentModel.DataAnnotations;
using TraineeManagement.Data.Enums;

namespace TraineeManagement.Data.DTOs
{
    public class TraineeResponseDTO 
    {
        public int? Id{get;set;}
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? Email { get; set; }
        public string? TechStack { get; set; }
        public TraineeStatusType? Status { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime UpdatedDate { get; set; }
    }
}
