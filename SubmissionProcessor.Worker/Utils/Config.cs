using SubmissionProcessor.Worker.Resources;

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
        DotNetEnv.Env.Load();
        IConfigurationSection rabbitSection = configuration.GetSection(StringConstants.RabbitMQSection);
        RabbitHostName = rabbitSection[StringConstants.HostNameKey] ?? StringConstants.DefaultLocalhost;
        RabbitPort = int.TryParse(rabbitSection[StringConstants.PortKey], out int port) ? port : 5672;
        RabbitUserName = rabbitSection[StringConstants.UserNameKey] ?? StringConstants.DefaultGuest;
        RabbitPassword = rabbitSection[StringConstants.PasswordKey] ?? StringConstants.DefaultGuest;
        RabbitVirtualHost = rabbitSection[StringConstants.VirtualHostKey] ?? StringConstants.DefaultMqHost;

        DirectoryApiBaseUrl = configuration[StringConstants.DirectoryApiBaseUrlKey] ?? StringConstants.DefaultDirectoryApiBaseUrl;
    }
}
