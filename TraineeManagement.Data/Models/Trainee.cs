using System.ComponentModel.DataAnnotations;
using TraineeManagement.Data.Enums;
using TraineeManagement.Data.DTOs;
using System.ComponentModel.DataAnnotations.Schema;
namespace TraineeManagement.Data.Models
{
    public class Trainee
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? Email { get; set; }
        public string? TechStack { get; set; }
        public TraineeStatusType? Status { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime UpdatedDate { get; set; }
        public ICollection<TaskAssignment>? TaskAssignments { get; set; }

        public Trainee(){}
        public Trainee(CreateTraineeRequestDTO trainee)
        {
            FirstName = trainee.FirstName;
            LastName = trainee.LastName;
            Email = trainee.Email;
            TechStack = trainee.TechStack;
            Status = trainee.Status;
            CreatedDate = DateTime.UtcNow;
            UpdatedDate = DateTime.UtcNow;
        }
    }
}
