namespace TraineeManagement1.DTOs
{
    public class SubmissionResponseDTO:CreateSubmissionRequestDTO
    {
        public int Id { get; set; }
        public DateTime SubmittedDate { get; set; }
    }
}