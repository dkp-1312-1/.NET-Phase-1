namespace TraineeManagement1.Models
{
    public class TaskAssignment
    {
        public int Id { get; set; }
         public int TraineeId { get; set; }
        // [ForeignKey("TraineeId")]
        public Trainee Trainee { get; set; }
 
        public int MentorId { get; set; }
        // [ForeignKey("MentorId")]
        public Mentor Mentor { get; set; }
 
        public int LearningTaskId { get; set; }
        // [ForeignKey("LearningTaskId")]
        public LearningTask LearningTask { get; set; }
        
        public DateTime AssignedDate { get; set; }
        public DateTime DueDate { get; set; }
        public string Status { get; set; } 
        public string Remarks { get; set; }
        public ICollection<Submission>? Submissions { get; set; }
    }
}