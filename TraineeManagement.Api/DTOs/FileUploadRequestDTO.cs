namespace TraineeManagement.Api.DTOs
{
    public class FileUploadRequestDTO
    {
        public int SubmissionId { get; set; }
        public IFormFile? File { get; set; }
        public int UserId { get; set; }
    }

}