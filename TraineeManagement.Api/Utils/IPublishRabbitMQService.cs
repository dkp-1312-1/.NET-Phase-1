using TraineeManagement.Api.DTOs;
namespace TraineeManagement.Api.Utils
{
    public interface IPublishRabbitMQService
    {
        Task<bool> PublishSubmission(SubmissionProcessingRequestedDTO message);
    }
}