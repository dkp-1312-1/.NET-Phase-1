using Microsoft.AspNetCore.Http;

namespace TraineeManagement.Data.DTOs
{
    public class FileUploadRequestDTO
    {
        public int SubmissionId { get; set; }
        public IFormFile? File { get; set; }
        public int UserId { get; set; }
        public string CorrelationId{get;set;}
    }

}
