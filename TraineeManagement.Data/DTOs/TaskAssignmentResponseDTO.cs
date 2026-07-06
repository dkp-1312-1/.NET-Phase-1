using TraineeManagement.Data.Enums;

namespace TraineeManagement.Data.DTOs
{
    public class TaskAssignmentResponseDTO
    {
        public int Id { get; set; }
        public int TraineeId { get; set; }
        public int MentorId { get; set; }
        public int LearningTaskId { get; set; }
        public DateTime AssignedDate { get; set; }
        public DateTime DueDate { get; set; }
        public TAType Status { get; set; }
        public string? Remarks { get; set; }
    }
}
