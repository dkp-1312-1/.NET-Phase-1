using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;

public class JWTService:IJWTService
{
    private readonly IConfiguration _configuration;
}