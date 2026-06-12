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

namespace TraineeManagement1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IJWTService _jwtService;
        public AuthController(IJWTService jwtService)
        {
            _jwtService = jwtService;
        }

        [HttpPost("login")]
        [ValidateModel]
        public async Task<IActionResult> Login([FromBody] LoginRequestDTO loginRequest)
        {
            try
            {
                LoginResponseDTO result =await _jwtService.GenerateToken(loginRequest);
                if(result==null)
                {
                    return Unauthorized(new { message = "Invalid username or password" });
                }
                Console.WriteLine(result.Token);
                return Ok(result);
            }
            catch(Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Something went wrong on our end."+ex.Message);
            }            
        }
    }
}