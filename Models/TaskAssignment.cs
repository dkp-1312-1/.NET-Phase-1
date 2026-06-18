namespace TraineeManagement1.Models
{
    public enum TAType
    {
        Assigned,
        Inprogress,
        Submitted,
        Reviewed,
        Completed
    }
    public class TaskAssignment
    {
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