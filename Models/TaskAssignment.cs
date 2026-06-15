namespace TraineeManagement1.Models
{
    public class TaskAssignment
    {
        public int Id { get; set; }
        public int TraineeId { get; set; }
        public int MentorId { get; set; }
        public int LearningTaskId { get; set; }
        public DateTime AssignedDate { get; set; }
        public DateTime DueDate { get; set; }
        public string Status { get; set; } 
        public string Remarks { get; set; }
    }
}