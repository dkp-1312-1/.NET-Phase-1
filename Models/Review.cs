namespace TraineeManagement1.Models
{
    public class Review
    {
        public required int Id { get; set; }
        public int SubmissionId { get; set; }
        // [ForeignKey("SubmissionId")]
        public Submission Submission { get; set; }
 
        public int MentorId { get; set; }
        // [ForeignKey("MentorId")]
        public Mentor Mentor { get; set; }
        
        public string Feedback { get; set; }
        public int? Score { get; set; }
        public string ReviewStatus { get; set; } 
        public DateTime ReviewedDate { get; set; }
    }
}