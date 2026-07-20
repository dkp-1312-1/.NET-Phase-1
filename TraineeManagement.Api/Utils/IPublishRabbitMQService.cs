using TraineeManagement.Data.DTOs;
namespace TraineeManagement.Api.Utils
{
    public interface IPublishRabbitMQService
    {
        Task<bool> PublishSubmission(SubmissionProcessingRequestedDTO message);
    }
}
