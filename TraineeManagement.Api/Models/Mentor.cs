using System.ComponentModel.DataAnnotations;
using TraineeManagement.Api.Enums;
using TraineeManagement.Api.DTOs;
using System.ComponentModel.DataAnnotations.Schema;
namespace TraineeManagement.Api.Models
{
    public class Mentor
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? Email { get; set; }
        public string? Expertise { get; set; }
        public MentorStatusType Status { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime UpdatedDate { get; set; }
        public ICollection<TaskAssignment>? TaskAssignments { get; set; }
        public ICollection<Review>? Reviews { get; set; }
        public Mentor(){}
        public Mentor(CreateMentorRequestDTO mentor)
        {
            FirstName = mentor.FirstName;
            LastName = mentor.LastName;
            Email = mentor.Email;
            Expertise = mentor.Expertise;
            Status = mentor.Status;
            CreatedDate = DateTime.UtcNow;
            UpdatedDate = DateTime.UtcNow;
        }
    }
}