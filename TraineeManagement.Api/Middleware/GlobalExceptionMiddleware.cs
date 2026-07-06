using TraineeManagement.Api.Utils;
using Microsoft.AspNetCore.Http;
using System.Net;
using System.Text.Json;
using TraineeManagement.Api.Services;
using MySql.Data.MySqlClient;
using TraineeManagement.Data.DTOs;
using TraineeManagement.Api.Resources;
using Google.Protobuf.WellKnownTypes;
using Microsoft.AspNetCore.Cors.Infrastructure;
using Microsoft.VisualBasic;

namespace TraineeManagement.Api.Middleware
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
                await WriteResponse(context, StatusCodes.Status404NotFound, ex.Message, true);
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
            catch (PayloadTooLargeException ex)
            {
                _logger.LogWarning("File Size Exceed: {Message}", ex.Message);
                await WriteResponse(context, StatusCodes.Status413RequestEntityTooLarge, ex.Message);
            }
            catch(ServiceUnavailableException ex)
            {
                _logger.LogError("Service is not available", ex.Message);
                await WriteResponse(context, StatusCodes.Status503ServiceUnavailable, ex.Message);
            }
            catch (Exception ex)
            {
                if (ex.InnerException is MySqlException mysqlEx)
                {
                    _logger.LogError($"{mysqlEx.Number}");
                    if (mysqlEx.Number == 1451)
                    {
                        _logger.LogWarning("Foreign key constraint failure on Delete: {Message}", mysqlEx.Message);
                        await WriteResponse(context, StatusCodes.Status400BadRequest, StringConstants.mysql1451);
                    }
                    if (mysqlEx.Number == 1452)
                    {
                        _logger.LogWarning("Foreign key constraint failure on Insert or Update: {Message}", mysqlEx.Message);
                        await WriteResponse(context, StatusCodes.Status400BadRequest, StringConstants.mysql1452);
                    }
                    if (mysqlEx.Number == 1062)
                    {
                        _logger.LogWarning("Duplicate entry constraint failure: {Message}", mysqlEx.Message);

                        if (mysqlEx.Message.Contains("Email"))
                        {
                            await WriteResponse(context, StatusCodes.Status409Conflict, StringConstants.mysqlEmail);
                        }
                        else if (mysqlEx.Message.Contains("Username"))
                        {
                            await WriteResponse(context, StatusCodes.Status409Conflict, StringConstants.mysqlUsername);
                        }
                        else
                        {
                            await WriteResponse(context, StatusCodes.Status409Conflict, StringConstants.mysqlDuplicate);
                        }
                    }
                }
                else
                {
                    _logger.LogError(ex, "Unhandled exception on {Method} {Path}",
                        context.Request.Method, context.Request.Path);
                    await WriteResponse(context, StatusCodes.Status500InternalServerError, StringConstants.InternalError);
                }

            }
        }

        private static async Task WriteResponse(HttpContext context, int statusCode, string message, bool? success = false)
        {
            context.Response.StatusCode = statusCode;
            context.Response.ContentType = "application/json";
            await context.Response.WriteAsJsonAsync(new { Message = message, Data = new List<String> { }, Success = success });
        }
    }
}
