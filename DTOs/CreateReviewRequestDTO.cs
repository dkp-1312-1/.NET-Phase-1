using TraineeManagement1.Models;

namespace TraineeManagement1.DTOs
{
    public class CreateReviewRequestDTO
    {
        public int SubmissionId { get; set; }
        public int MentorId { get; set; }
        public string Feedback { get; set; }
        public int? Score { get; set; }
        public RSType ReviewStatus { get; set; }
    }
}