using TraineeManagement.Data.Enums;
using TraineeManagement.Data.DTOs;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace TraineeManagement.Data.Models
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

        public DateTime AssignedDate { get; set; } = DateTime.UtcNow;
        public DateTime DueDate { get; set; }
        public TAType Status { get; set; } = TAType.Assigned;
        public string Remarks { get; set; }
        public ICollection<Submission>? Submissions { get; set; }

        public TaskAssignment(){}
        public TaskAssignment(CreateTaskAssignmentRequestDTO request)
        {
            TraineeId = request.TraineeId;
            MentorId = request.MentorId;
            LearningTaskId = request.LearningTaskId;
            DueDate = request.DueDate;
            Remarks = request.Remarks;
            AssignedDate = DateTime.UtcNow;
            Status = TAType.Assigned;
        }
    }
}
