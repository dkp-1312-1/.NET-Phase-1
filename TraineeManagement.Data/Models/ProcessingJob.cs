using TraineeManagement.Data.Enums;
using TraineeManagement.Data.DTOs;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace TraineeManagement.Data.Models
{
    public class ProcessingJob
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id{get;set;}
        public string MessageId{get;set;}
        public string? CorrelationId {get;set;}
        public int SubmissionId {get;set;}

        public Submission? Submission {get;set;}
        public ProcessingJobType Status{get;set;}
        public int Attempts{get;set;}
        public string?ErrorSummary{get;set;}

        public DateTime StartedAt{get;set;}
        public DateTime CompletedAt{get;set;}

        public ProcessingJob(){}
        public ProcessingJob(SubmissionProcessingRequestedDTO request)
        {
            MessageId=request.MessageId;
            CorrelationId=request.CorrelationId;
            SubmissionId=request.SubmissionId;
            Status=ProcessingJobType.Queued;
            Attempts=0;
        }
    }
}
