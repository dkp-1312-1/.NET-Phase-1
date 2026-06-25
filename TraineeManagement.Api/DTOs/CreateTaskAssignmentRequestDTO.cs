using System.ComponentModel.DataAnnotations;
namespace TraineeManagement.Api.DTOs
{
    public class CreateTaskAssignmentRequestDTO
    {
        [Required(ErrorMessage = "TraineeId must exist")]
        public int TraineeId { get; set; }
        [Required(ErrorMessage = "MentorId must exist")]
        public int MentorId { get; set; }
        [Required(ErrorMessage = "LearningTaskId must exist")]
        public int LearningTaskId { get; set; }
        public DateTime DueDate { get; set; }
        public string Remarks { get; set; }
    }
}