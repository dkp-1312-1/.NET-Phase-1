using Microsoft.AspNetCore.Http;
using System.Net;
using System.Text.Json;
using TraineeManagement1.Services;

namespace TraineeManagement1.Middleware
{
    public class GlobalExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<GlobalExceptionMiddleware> _logger;

        public GlobalExceptionMiddleware(RequestDelegate next, ILogger<GlobalExceptionMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }
        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (NotFoundException ex)
            {
                _logger.LogWarning("Not found: {Message}", ex.Message);
                await WriteResponse(context, StatusCodes.Status404NotFound, ex.Message);
            }
            catch (UnauthorizedException ex)
            {
                _logger.LogWarning("Unauthorized: {Message}", ex.Message);
                await WriteResponse(context, StatusCodes.Status401Unauthorized, ex.Message);
            }
            catch (BadRequestException ex)
            {
                _logger.LogWarning("Bad request: {Message}", ex.Message);
                await WriteResponse(context, StatusCodes.Status400BadRequest, ex.Message);
            }
            catch (JwtOperationException ex)
            {
                _logger.LogError(ex, "Invalid operation: {Message}", ex.Message);
                await WriteResponse(context, StatusCodes.Status500InternalServerError, "An unexpected error occurred while processing authentication please retry");
            }
            catch (Exception ex)
            {
                await HandleExceptionAsync(context);
            }
        }
        private static Task HandleExceptionAsync(HttpContext context)
        {
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

            var response = new
            {
                message = "An error occurred while processing your request."
            };
            return context.Response.WriteAsync(JsonSerializer.Serialize(response));
        }
        private static async Task WriteResponse(HttpContext context, int statusCode, string message)
        {
            context.Response.StatusCode = statusCode;
            context.Response.ContentType = "application/json";
            await context.Response.WriteAsJsonAsync(new { Message = message}); 
        }
    }
}