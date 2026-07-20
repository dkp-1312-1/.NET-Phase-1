using TraineeManagement.Data.Enums;
using TraineeManagement.Data.DTOs;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace TraineeManagement.Data.Models
{
    public class Review
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public int SubmissionId { get; set; }
        public Submission Submission { get; set; }

        public int MentorId { get; set; }
        public Mentor Mentor { get; set; }

        public string Feedback { get; set; }
        public int? Score { get; set; }
        public RSType ReviewStatus { get; set; }
        public DateTime ReviewedDate { get; set; }

        public Review(){}
        public Review(CreateReviewRequestDTO request)
        {
            SubmissionId = request.SubmissionId;
            MentorId = request.MentorId;
            Feedback = request.Feedback;
            Score = request.Score;
            ReviewStatus = request.ReviewStatus;
            ReviewedDate = DateTime.UtcNow;
        }
    }
}
