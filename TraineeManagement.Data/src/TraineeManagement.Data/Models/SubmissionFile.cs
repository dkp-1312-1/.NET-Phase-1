namespace TraineeManagement.Data.Models
{
    public class SubmissionFile
    {
        public int Id { get; set; }
        public int SubmissionId { get; set; }
        public Submission? Submission { get; set; }
        public string? OriginalFileName { get; set; }
        public string? StorageFileName { get; set; }
        public string? ContentType { get; set; }
        public long SizeBytes { get; set; }
        public string? CheckSum { get; set; }
        public int UploadedByUserId { get; set; }
        public DateTime UploadedDate{ get; set; }
    }
}
