namespace TraineeManagement1.Models
{
    public class Submission
    {
        public required int Id { get; set; }
        public int TaskAssignmentId { get; set; }
        // [ForeignKey("TaskAssignmentId")]
        public TaskAssignment TaskAssignment { get; set; }
        public string SubmissionUrl { get; set; }
        public string Notes { get; set; }
        public DateTime SubmittedDate { get; set; }
        public string Status { get; set; } 
        public ICollection<Review> Reviews { get; set; }
    }
}