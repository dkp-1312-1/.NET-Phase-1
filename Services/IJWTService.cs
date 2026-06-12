using System.Security.Claims;

public interface IJwtService
{
    string GenerateToken(string username, string role);
}
