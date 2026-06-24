namespace TraineeManagement.Api.DTOs
{
    public class SubmissionProcessingRequestedDTO
    {
        public string MessageId{get;set;}= Guid.NewGuid().ToString();
        public string CorrelationId{get;set;}
        public int SubmissionId{get;set;}
        public int  FileId{get;set;}
        public DateTime RequestedAt{get;set;} 
        public string ContractVersion {get;set;}="1.0";
    }
}