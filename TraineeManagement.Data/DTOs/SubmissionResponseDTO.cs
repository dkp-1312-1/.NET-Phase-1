namespace TraineeManagement.Data.DTOs
{
    public class SubmissionResponseDTO:CreateSubmissionRequestDTO
    {
        public int Id { get; set; }
        public DateTime SubmittedDate { get; set; }
    }
}
