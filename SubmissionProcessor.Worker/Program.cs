using SubmissionProcessor.Worker;
using Microsoft.EntityFrameworkCore;
using TraineeManagement.Data.DTOs;
using TraineeManagement.Api.Services;
using TraineeManagement.Data.Data;
using TraineeManagement.Api.Repositories;
using MySql.EntityFrameworkCore;
using Polly;
using Polly.Extensions.Http;
using SubmissionProcessor.Worker.Services;
using TraineeManagement.Api.Utils;
using Polly.Retry;
using Polly.CircuitBreaker;
using Serilog;
using SubmissionProcessor.Worker.Resources;

namespace SubmissionProcessor.Worker
{
    public class Program
    {
        public static void Main(string[] args)
        {
            HostApplicationBuilder builder = Host.CreateApplicationBuilder(args);

builder.Configuration.AddJsonFile(Path.Combine(builder.Environment.ContentRootPath, "..", StringConstants.AppSettingsJson), optional: true, reloadOnChange: true);
builder.Configuration.AddJsonFile(Path.Combine(builder.Environment.ContentRootPath, "..", $"appsettings.{builder.Environment.EnvironmentName}.json"), optional: true, reloadOnChange: true);

SubmissionProcessor.Worker.Utils.Config.Initialize(builder.Configuration);

AsyncRetryPolicy<HttpResponseMessage> retryPolicy = HttpPolicyExtensions
    .HandleTransientHttpError() 
    .WaitAndRetryAsync(3, retryAttempt => TimeSpan.FromSeconds(2));
AsyncCircuitBreakerPolicy<HttpResponseMessage> circuitBreakerPolicy = HttpPolicyExtensions
    .HandleTransientHttpError()
    .CircuitBreakerAsync(5, TimeSpan.FromSeconds(30));
string redisString = builder.Configuration.GetConnectionString(StringConstants.RedisConnectionKey)
    ?? throw new InvalidOperationException(StringConstants.RedisConnectionNotFound);

builder.Services.AddStackExchangeRedisCache(options =>
{
    options.Configuration = redisString;
    options.InstanceName = StringConstants.RedisInstanceName;
});
builder.Services.AddHttpClient(StringConstants.DirectoryApiClientName,client =>
{
    client.BaseAddress = new Uri(SubmissionProcessor.Worker.Utils.Config.DirectoryApiBaseUrl); 
    client.Timeout = TimeSpan.FromSeconds(5); 
})
.AddPolicyHandler(retryPolicy)
.AddPolicyHandler(circuitBreakerPolicy);

builder.Services.AddScoped<TrainingDirectoryClient>();

string connectionString = builder.Configuration.GetConnectionString(StringConstants.DefaultConnectionKey)
    ?? throw new InvalidOperationException(StringConstants.DefaultConnectionNotFound);
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseMySQL(connectionString));
    
builder.Logging.ClearProviders();
Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Information()
   .WriteTo.Logger(lc => lc
        .Filter.ByExcluding(logEvent =>
            logEvent.Properties.TryGetValue("SourceContext", out var value) &&
            (value.ToString().Contains("Microsoft") 
            || value.ToString().Contains("System")
            || value.ToString().Contains("MySql")
            || value.ToString().Contains("Microsoft.EntityFrameworkCore.Database")))
        .WriteTo.Console()) 
    .WriteTo.File(
        path: StringConstants.LogFilePath,
        rollingInterval: RollingInterval.Day,
        outputTemplate: StringConstants.LogOutputTemplate)        
    .CreateLogger();
    
builder.Logging.AddSerilog(Log.Logger);
builder.Services.AddHostedService<Worker>();

IHost host = builder.Build();
            host.Run();
        }
    }
}
