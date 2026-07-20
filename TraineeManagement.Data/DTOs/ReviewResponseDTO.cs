using TraineeManagement.Data.Enums;

namespace TraineeManagement.Data.DTOs
{
    public class ReviewResponseDTO
    {
        public int Id { get; set; }
        public int SubmissionId { get; set; }
        public int MentorId { get; set; }
        public string? Feedback { get; set; }
        public int? Score { get; set; }
        public RSType ReviewStatus { get; set; }
        public DateTime ReviewedDate { get; set; }
    }
}
