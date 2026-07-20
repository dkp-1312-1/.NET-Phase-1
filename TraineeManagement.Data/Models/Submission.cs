using System.Security.Permissions;
using TraineeManagement.Data.Enums;
using TraineeManagement.Data.DTOs;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace TraineeManagement.Data.Models
{
    public class Submission
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public int TaskAssignmentId { get; set; }
        public TaskAssignment TaskAssignment { get; set; }
        public string SubmissionUrl { get; set; }
        public string Notes { get; set; }
        public DateTime SubmittedDate { get; set; }
        public SubType Status { get; set; }
        public ICollection<Review> Reviews { get; set; }
        public ICollection<SubmissionFile> SubmissionFiles { get; set; }
        public ICollection<ProcessingJob> ProcessingJobs { get; set; }

        public Submission(){}
        public Submission(CreateSubmissionRequestDTO request)
        {
            TaskAssignmentId = request.TaskAssignmentId;
            SubmissionUrl = request.SubmissionUrl;
            Notes = request.Notes;
            SubmittedDate = DateTime.UtcNow;
            Status = request.Status;
        }
    }
}
