using System.Text;
using Microsoft.IdentityModel.Tokens;

public static class Config
{
    public static SymmetricSecurityKey SecurityKey = null!;
    public static string Issuer = string.Empty;
    public static string Audience = string.Empty;
    public static int ExpiryIn = 60;
    public static string StorageRoot = string.Empty;

    public static string RabbitHostName=string.Empty;
    public static int RabbitPort;
    public static string RabbitUserName=string.Empty;
    public static string RabbitPassword=string.Empty;
    public static string RabbitVirtualHost=string.Empty;


    public static void Initialize(IConfiguration configuration)
    {
        var section = configuration.GetSection("Jwt");
        JWTIssuer = section["Issuer"];
        JWTAudience = section["Audience"];
        JWTExpiryInMinutes = int.Parse(section["ExpiryIn"]) * 60;
        SecurityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(section["Key"]));
        var fileSection = configuration.GetSection("FileStorage");
        StorageRoot = fileSection["RootPath"];
        if (!Directory.Exists(StorageRoot))
        {
            Directory.CreateDirectory(StorageRoot);
        }
        var rabbitSection=configuration.GetSection("RabbitMQ");
        RabbitHostName=rabbitSection["HostName"];
        RabbitPort=int.Parse(rabbitSection["Port"]);
        RabbitUserName=rabbitSection["UserName"];
        RabbitPassword=rabbitSection["Password"];
        RabbitVirtualHost=rabbitSection["VirtualHost"];

    }

}
