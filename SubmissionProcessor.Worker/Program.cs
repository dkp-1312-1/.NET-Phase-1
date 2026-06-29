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

var retryPolicy = HttpPolicyExtensions
    .HandleTransientHttpError() // Handles 5xx errors and network failures
    .WaitAndRetryAsync(3, retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)));
var circuitBreakerPolicy = HttpPolicyExtensions
    .HandleTransientHttpError()
    .CircuitBreakerAsync(5, TimeSpan.FromSeconds(30));

builder.Services.AddHttpClient<TrainingDirectoryClient>(client =>
{
    // Centralized configuration (Task 3.18)
    client.BaseAddress = new Uri("http://localhost:5000/"); // Update to match your Directory API port
    client.Timeout = TimeSpan.FromSeconds(5); // Finite Timeout (Task 3.19)
})
.AddPolicyHandler(retryPolicy)
.AddPolicyHandler(circuitBreakerPolicy);



var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseMySQL(connectionString));
builder.Services.AddScoped<IFileStorageService, LocalFileStorageService>();
builder.Services.AddScoped<IPublishRabbitMQService, PublishRabbitMQService>();
builder.Services.AddScoped<ISubmissionFileRepository, SubmissionFileRepository>();
builder.Services.AddScoped<IProcessingJobRepository, ProcessingJobRepository>();


builder.Services.AddHostedService<Worker>();

var host = builder.Build();
host.Run();
