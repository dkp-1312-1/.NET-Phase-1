using System.ComponentModel.DataAnnotations;
using TraineeManagement.Api.Enums;
namespace TraineeManagement.Api.Models
{
    public class Trainee
    {
        public required int Id { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? Email { get; set; }
        public string? TechStack { get; set; }
        public TraineeStatusType? Status { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime UpdatedDate { get; set; }
        public ICollection<TaskAssignment>? TaskAssignments { get; set; }
    }
}