using System.Text;
using Microsoft.IdentityModel.Tokens;

namespace TraineeManagement.Api.Utils;

public static class Config
{
    public static SymmetricSecurityKey JWTSecurityKey = null!;
    public static string JWTIssuer = string.Empty;
    public static string JWTAudience = string.Empty;
    public static int JWTExpiryInSeconds=3600;
    public static string StorageRoot = string.Empty;

    public static int StorageFileSizeLimit;
    public static int RedisCacheTimeLimit;

    public static string RabbitHostName=string.Empty;
    public static int RabbitPort;
    public static string RabbitUserName=string.Empty;
    public static string RabbitPassword=string.Empty;
    public static string RabbitVirtualHost=string.Empty;
    public static string[] StorageAllowedExtensions={};


    public static void Initialize(IConfiguration configuration)
    {
        DotNetEnv.Env.Load();
        IConfigurationSection section = configuration.GetSection("Jwt");
        JWTIssuer = section["Issuer"];
        JWTAudience = section["Audience"];
        JWTExpiryInSeconds = int.Parse(section["ExpiryMinutes"])*60;
        JWTSecurityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(section["Key"]));
        IConfigurationSection fileSection = configuration.GetSection("FileStorage");
        StorageRoot = fileSection["RootPath"];
        StorageFileSizeLimit=int.Parse(fileSection["FileSizeLimit"]);
        StorageAllowedExtensions=fileSection.GetSection("AllowedExtensions").Get<string[]>() ?? new string[0];

        if (!Directory.Exists(StorageRoot))
        {
            Directory.CreateDirectory(StorageRoot);
        }
        IConfigurationSection rabbitSection =configuration.GetSection("RabbitMQ");
        RabbitHostName=rabbitSection["HostName"];
        RabbitPort=int.Parse(rabbitSection["Port"]);
        RabbitUserName=rabbitSection["UserName"];
        RabbitPassword=rabbitSection["Password"];
        RabbitVirtualHost=rabbitSection["VirtualHost"];
        
        IConfigurationSection redisSection =configuration.GetSection("Redis");
        RedisCacheTimeLimit=int.Parse(redisSection["CacheTimeLimit"]);
    }

}
