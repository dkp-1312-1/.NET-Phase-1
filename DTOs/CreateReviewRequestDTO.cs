using TraineeManagement.Api.Enums;

namespace TraineeManagement.Api.DTOs
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