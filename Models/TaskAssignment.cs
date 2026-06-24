using TraineeManagement.Api.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace TraineeManagement.Api.Models
{
    public class TaskAssignment
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public int TraineeId { get; set; }
        public Trainee Trainee { get; set; }

        public int MentorId { get; set; }

        public Mentor Mentor { get; set; }

        public int LearningTaskId { get; set; }
        public LearningTask LearningTask { get; set; }

        public DateTime AssignedDate { get; set; }
        public DateTime DueDate { get; set; }
        public TAType Status { get; set; }
        public string Remarks { get; set; }
        public ICollection<Submission>? Submissions { get; set; }
    }
}