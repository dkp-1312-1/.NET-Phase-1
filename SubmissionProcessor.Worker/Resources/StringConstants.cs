namespace SubmissionProcessor.Worker.Resources;

public static class StringConstants
{
    public const string DeadLetterExchange = "dead-letter-exchange";
    public const string SubmissionFailedQueue = "submission-failed";
    public const string SubmissionFailedRoutingKey = "submission-failed";
    public const string SubmissionProcessingQueue = "submission-processing";

    public const string RequestNotFound = "Request was not found.";
    public const string JobNotFound = "Job {MessageId} not found.";
    public const string FileRecordNotFound = "File record not found.";
    public const string ChecksumNotMatched = "Checksum not matched: Cannot Process File.";

    public const string AttemptingConnect = "Attempting to connect to RabbitMQ (Attempt {Attempt}/{MaxRetries})...";
    public const string SuccessfullyConnected = "Successfully connected to RabbitMQ.";
    public const string FailedConnect = "Failed to connect to RabbitMQ on attempt {Attempt}.";
    public const string ExhaustedAttempts = "Exhausted all {MaxRetries} attempts to connect to RabbitMQ. Throwing exception.";
    public const string WaitingRetry = "Waiting {DelaySeconds} seconds before retrying...";
    public const string GoingToProcessQueue = "Going to Process Queue";
    
    public const string JobAlreadyStatus = "Job {Id} already {Status}. Skipping...";
    public const string ProcessingJob = "Processing Job {Id}. Attempt {Attempt}";
    public const string SuccessfullyRetrievedProfile = "Successfully retrieved profile for processing: {Profile}";
    public const string FallbackActivated = "Fallback activated: Processing file without Trainee Profile data.";
    public const string ChecksumMatched = "Checksum matched: Process File.";
    public const string JobSuccessfullyCompleted = "Job {Id} successfully completed with Checksum: {Checksum}";
    public const string ErrorProcessingJob = "Error processing Job {Id}";
    public const string JobExhaustedRetries = "Job {Id} exhausted retries.Moved to Dead Letter Queue.";
}
