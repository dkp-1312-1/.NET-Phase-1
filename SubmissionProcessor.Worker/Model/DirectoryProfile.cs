namespace SubmissionProcessor.Worker.Models
{
    public class DirectoryProfile
    {
        public int TraineeId { get; set; }
        public string Department { get; set; } = string.Empty;
        public string MentorName { get; set; } = string.Empty;
        public bool IsActive { get; set; }
    }
}