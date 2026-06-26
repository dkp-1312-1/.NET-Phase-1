using TraineeManagement.Api.Enums;
namespace TraineeManagement.Api.DTOs
{
    public class ProcessingJobResponseDTO
    {
        public int Id{get;set;}
        public string MessageId{get;set;}
        public string? CorrelationId {get;set;}
        public int SubmissionId {get;set;}
        public ProcessingJobType Status{get;set;}
        public int Attempts{get;set;}
        public string?ErrorSummary{get;set;}

        public DateTime StartedAt{get;set;}
        public DateTime CompletedAt{get;set;}
    }

}