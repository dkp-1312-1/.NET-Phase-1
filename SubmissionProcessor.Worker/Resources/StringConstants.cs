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

    public const string ExchangeTypeDirect = "direct";
    public const string XDeadLetterExchange = "x-dead-letter-exchange";
    public const string XDeadLetterRoutingKey = "x-dead-letter-routing-key";
    public const string CorrelationIdLogScope = "=>=>CorrelationId<=<= {CorrelationId}";
    public const string UploadsDirectory = "/app/uploads";
    public const string FileNotFoundInStorage = "File not found in storage: {FullPath}";

    public const string DirectoryApiClientName = "DirectoryApi";
    public const string CorrelationIdHeader = "X-Correlation-ID";
    public const string DirectoryApiReturnedError = "Directory API returned {StatusCode} for Trainee {Id}";
    public const string DirectoryApiTimeout = "Directory API request timed out for Trainee {Id}";
    public const string DirectoryApiFailed = "Failed to call Directory API.";

    public const string AppSettingsJson = "appsettings.json";
    public const string RedisConnectionKey = "RedisConnection";
    public const string RedisConnectionNotFound = "RedisConnection string not found.";
    public const string RedisInstanceName = "TraineeManagement";
    public const string DefaultConnectionKey = "DefaultConnection";
    public const string DefaultConnectionNotFound = "DefaultConnection string not found.";
    public const string LogFilePath = "logs/app-.txt";
    public const string LogOutputTemplate = "{Timestamp:yyyy-MM-dd HH:mm:ss.fff zzz} [{Level:u3}] {Message:lj}{NewLine}";

    public const string RabbitMQSection = "RabbitMQ";
    public const string HostNameKey = "HostName";
    public const string PortKey = "Port";
    public const string UserNameKey = "UserName";
    public const string PasswordKey = "Password";
    public const string VirtualHostKey = "VirtualHost";
    public const string DefaultLocalhost = "localhost";
    public const string DefaultGuest = "guest";
    public const string DefaultMqHost = "mqhost";
    public const string DirectoryApiBaseUrlKey = "DirectoryApi:BaseUrl";
    public const string DefaultDirectoryApiBaseUrl = "http://training_directory_api:8080/";
}
