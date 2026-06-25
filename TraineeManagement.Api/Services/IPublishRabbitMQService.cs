using TraineeManagement.Api.DTOs;
namespace TraineeManagement.Api.Services
{
    public interface IPublishRabbitMQService
    {
        void PublishSubmission(SubmissionProcessingRequestedDTO message);
    }
}