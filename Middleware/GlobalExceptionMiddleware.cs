using Microsoft.AspNetCore.Http;
using System.Net;
using System.Text.Json;
using TraineeManagement1.Services;
using MySql.Data.MySqlClient;
using TraineeManagement1.DTOs;
using Google.Protobuf.WellKnownTypes;

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
                if (ex.InnerException is MySqlException mysqlEx)
                {
                    _logger.LogError($"ERROR CODE::::{mysqlEx.Number}::::::::::");
                    if (mysqlEx.Number == 1451)
                    {
                        _logger.LogWarning("Foreign key constraint failure on Delete: {Message}", mysqlEx.Message);
                        await WriteResponse(context, StatusCodes.Status400BadRequest, "Cannot delete or update because of related data. Please remove related data first or change reference");
                    }
                    if (mysqlEx.Number == 1452) 
                    {
                        _logger.LogWarning("Foreign key constraint failure on Insert or Update: {Message}", mysqlEx.Message);
                        await WriteResponse(context, StatusCodes.Status400BadRequest, "Related data not found, Please ensure referenced data exists..");
                    }
                    if (mysqlEx.Number == 1062) 
                    {
                        _logger.LogWarning("Duplicate entry constraint failure: {Message}", mysqlEx.Message);

                        if (mysqlEx.Message.Contains("Email"))
                        {
                            await WriteResponse(context, StatusCodes.Status409Conflict, "This email address is already registered.");
                        }
                        else if (mysqlEx.Message.Contains("Username"))
                        {
                            await WriteResponse(context, StatusCodes.Status409Conflict, "Username already exists.");
                        }
                        else
                        {
                            await WriteResponse(context, StatusCodes.Status409Conflict, "A record with these details already exists.");
                        }
                    }
                }
                else
                {
                    _logger.LogError(ex, "Unhandled exception on {Method} {Path}",
                        context.Request.Method, context.Request.Path);
                    await WriteResponse(context, StatusCodes.Status500InternalServerError, "Something Went Wrong, Please Try Again");
                }

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
            if(statusCode==404)
            {
                await context.Response.WriteAsJsonAsync(new {Message=message,Data=new List<String>{},Success=false});
            }
            else
            {
                await context.Response.WriteAsJsonAsync(new { Message = message });
            }
            
        }
    }
}