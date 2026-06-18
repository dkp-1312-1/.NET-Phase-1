using System.ComponentModel.DataAnnotations;
using TraineeManagement1.Models;

namespace TraineeManagement1.DTOs
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
