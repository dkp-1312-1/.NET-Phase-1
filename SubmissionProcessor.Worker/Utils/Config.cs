namespace SubmissionProcessor.Worker.Utils;

public static class Config
{
    public static string RabbitHostName = string.Empty;
    public static int RabbitPort;
    public static string RabbitUserName = string.Empty;
    public static string RabbitPassword = string.Empty;
    public static string RabbitVirtualHost = string.Empty;
    public static string DirectoryApiBaseUrl = string.Empty;

    public static void Initialize(IConfiguration configuration)
    {
        IConfigurationSection rabbitSection = configuration.GetSection("RabbitMQ");
        RabbitHostName = rabbitSection["HostName"] ?? "localhost";
        RabbitPort = int.TryParse(rabbitSection["Port"], out var port) ? port : 5672;
        RabbitUserName = rabbitSection["UserName"] ?? "guest";
        RabbitPassword = rabbitSection["Password"] ?? "guest";
        RabbitVirtualHost = rabbitSection["VirtualHost"] ?? "mqhost";

        DirectoryApiBaseUrl = configuration["DirectoryApi:BaseUrl"] ?? "http://training_directory_api:8080/";
    }
}
