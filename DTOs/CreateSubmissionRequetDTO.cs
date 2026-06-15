namespace TraineeManagement1.DTOs
{
    public class CreateSubmissionRequestDTO
    {
        public int TaskAssignmentId { get; set; }
        public string SubmissionUrl { get; set; }
        public string Notes { get; set; }
        public string Status { get; set; }
    }
}
