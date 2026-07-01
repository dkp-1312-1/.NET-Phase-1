using SubmissionProcessor.Worker;
using Microsoft.EntityFrameworkCore;
using TraineeManagement.Api.DTOs;
using TraineeManagement.Api.Services;
using TraineeManagement.Api.Data;
using TraineeManagement.Api.Repositories;
using MySql.EntityFrameworkCore;
using Polly;
using Polly.Extensions.Http;
using SubmissionProcessor.Worker.Services;
using TraineeManagement.Api.Utils;

var builder = Host.CreateApplicationBuilder(args);

builder.Configuration.AddJsonFile(Path.Combine(builder.Environment.ContentRootPath, "..", "appsettings.json"), optional: true, reloadOnChange: true);
builder.Configuration.AddJsonFile(Path.Combine(builder.Environment.ContentRootPath, "..", $"appsettings.{builder.Environment.EnvironmentName}.json"), optional: true, reloadOnChange: true);

var retryPolicy = HttpPolicyExtensions
    .HandleTransientHttpError() 
    .WaitAndRetryAsync(3, retryAttempt => TimeSpan.FromSeconds(2));
var circuitBreakerPolicy = HttpPolicyExtensions
    .HandleTransientHttpError()
    .CircuitBreakerAsync(5, TimeSpan.FromSeconds(30));

builder.Services.AddHttpClient<TrainingDirectoryClient>(client =>
{
    var directoryApiBaseUrl = builder.Configuration["DirectoryApi:BaseUrl"] ?? "http://training_directory_api:8080/";
    client.BaseAddress = new Uri(directoryApiBaseUrl); 
    client.Timeout = TimeSpan.FromSeconds(5); 
})
.AddPolicyHandler(retryPolicy)
.AddPolicyHandler(circuitBreakerPolicy);



var connectionString = builder.Configuration.GetConnectionString("DefaultConnection")
    ?? throw new InvalidOperationException("DefaultConnection string not found.");
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseMySQL(connectionString));
builder.Services.AddScoped<IFileStorageService, LocalFileStorageService>();
builder.Services.AddScoped<IPublishRabbitMQService, PublishRabbitMQService>();
builder.Services.AddScoped<ISubmissionFileRepository, SubmissionFileRepository>();
builder.Services.AddScoped<IProcessingJobRepository, ProcessingJobRepository>();
builder.Logging.ClearProviders();
builder.Logging.AddSimpleConsole(options =>
{
    options.IncludeScopes = true; 
});
builder.Services.AddHostedService<Worker>();

var host = builder.Build();
host.Run();