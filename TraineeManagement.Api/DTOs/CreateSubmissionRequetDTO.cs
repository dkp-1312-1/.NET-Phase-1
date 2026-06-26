using TraineeManagement.Api.Enums;

namespace TraineeManagement.Api.DTOs
{
    public class CreateSubmissionRequestDTO
    {
        public int TaskAssignmentId { get; set; }
        public string? SubmissionUrl { get; set; }
        public string? Notes { get; set; }
        public SubType Status { get; set; }
    }
}
