using TraineeManagement.Api.Enums;
namespace TraineeManagement.Api.Models
{
    public class Review
    {
        public required int Id { get; set; }
        public int SubmissionId { get; set; }
        public Submission Submission { get; set; }
 
        public int MentorId { get; set; }
        public Mentor Mentor { get; set; }
        
        public string Feedback { get; set; }
        public int? Score { get; set; }
        public RSType ReviewStatus { get; set; } 
        public DateTime ReviewedDate { get; set; }
    }
}