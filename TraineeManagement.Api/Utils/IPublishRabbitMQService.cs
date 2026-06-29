using TraineeManagement.Api.DTOs;
namespace TraineeManagement.Api.Utils
{
    public interface IPublishRabbitMQService
    {
        void PublishSubmission(SubmissionProcessingRequestedDTO message);
    }
}