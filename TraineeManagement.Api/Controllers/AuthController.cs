using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using TraineeManagement.Api.DTOs;
using TraineeManagement.Api.Models;
using TraineeManagement.Api.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using TraineeManagement.Api.Data;
using BCrypt.Net;
using Microsoft.Extensions.Localization;
using TraineeManagement.Api.Resources;

namespace TraineeManagement.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly ILogger<AuthController> _logger;
        private readonly IJWTService _jwtService;
        public AuthController(ILogger<AuthController> logger, IJWTService jwtService)
        {
            _logger = logger;
            _jwtService = jwtService;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequestDTO loginRequest)
        {
            LoginResponseDTO result = await _jwtService.GenerateToken(loginRequest);
            if (result == null)
            {
                throw new UnauthorizedException(StringConstants.Unauthorized);
            }
            _logger.LogInformation("Successful login for username: {Username}", loginRequest.Username);
            return Ok(result);
        }
    }
}