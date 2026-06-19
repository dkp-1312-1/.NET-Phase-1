using System.Text;
using Microsoft.IdentityModel.Tokens;

public static class Config
{
    public static SymmetricSecurityKey SecurityKey = null!;
    public static string Issuer = string.Empty;
    public static string Audience =  string.Empty;
    public static int ExpiryIn = 60;

    public static void Initialize(IConfiguration configuration)
    {
        var section = configuration.GetSection("Jwt");
        Issuer = section["Issuer"];
        Audience = section["Audience"];
        ExpiryIn = int.Parse(section["ExpiryIn"])*60;
        SecurityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(section["Key"]));
    }
}
