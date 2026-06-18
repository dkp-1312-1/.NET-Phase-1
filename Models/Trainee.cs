using System.ComponentModel.DataAnnotations;
namespace TraineeManagement1.Models
{
    public enum TraineeStatusType
    {
        Active,
        Inactive,
    }
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