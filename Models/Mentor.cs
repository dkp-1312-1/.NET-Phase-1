using System.ComponentModel.DataAnnotations;
namespace TraineeManagement1.Models
{
    public enum MentorStatusType
    {
        Active,
        Inactive
    }
    public class Mentor
    {
        public required int Id { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? Email { get; set; }
        public string? Expertise { get; set; }
        public MentorStatusType? Status { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime UpdatedDate { get; set; }
        public ICollection<TaskAssignment>? TaskAssignments { get; set; }
        public ICollection<Review>? Reviews { get; set; }
    }
}