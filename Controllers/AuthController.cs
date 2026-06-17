using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using TraineeManagement1.DTOs;
using TraineeManagement1.Models;
using TraineeManagement1.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using TraineeManagement1.Data;
using BCrypt.Net;
using Microsoft.Extensions.Localization;
using TraineeManagement1.Resources;

namespace TraineeManagement1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly ILogger<AuthController> _logger;
        private readonly IJWTService _jwtService;
        private readonly IStringLocalizer<SharedResource> _localizer;
        public AuthController(ILogger<AuthController> logger, IJWTService jwtService,IStringLocalizer<SharedResource> localizer)
        {
            _logger = logger;
            _jwtService = jwtService;
            _localizer=localizer;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequestDTO loginRequest)
        {
            LoginResponseDTO result = await _jwtService.GenerateToken(loginRequest);
            if (result == null)
            {
                throw new UnauthorizedException(_localizer["Unauthorized"]);
            }
            _logger.LogInformation("Successful login for username: {Username}", loginRequest.Username);
            return Ok(result);
        }
    }
}