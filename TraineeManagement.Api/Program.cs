using TraineeManagement.Api.Utils;
using System.Reflection.Metadata;
using TraineeManagement.Api.Services;
using TraineeManagement.Api.Data;
using TraineeManagement.Api.Repositories;
using TraineeManagement.Api.Middleware;
using TraineeManagement.Api.Resources;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.Text.Json;
using Microsoft.OpenApi.Models;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using RabbitMQ.Client;
var builder = WebApplication.CreateBuilder(args);

builder.Configuration.AddJsonFile(Path.Combine(builder.Environment.ContentRootPath, "..", "appsettings.json"), optional: true, reloadOnChange: true);
builder.Configuration.AddJsonFile(Path.Combine(builder.Environment.ContentRootPath, "..", $"appsettings.{builder.Environment.EnvironmentName}.json"), optional: true, reloadOnChange: true);

Config.Initialize(builder.Configuration);

builder.Services.AddCors(options =>
{
    options.AddPolicy("ReactClientPolicy", policy =>
    {
        policy.WithOrigins("http://localhost:3000", "http://localhost:5173")
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});



builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ClockSkew = TimeSpan.Zero,
        ValidateIssuerSigningKey = true,
        ValidIssuer = Config.JWTIssuer,
        ValidAudience = Config.JWTAudience,
        IssuerSigningKey =Config.JWTSecurityKey
    };
    options.Events = new JwtBearerEvents
        {
            OnChallenge=async context =>
            {
                throw new UnauthorizedException(StringConstants.JWTUnauthorized);
            }
        };
});
builder.Services.AddAuthorization();

builder.Logging.ClearProviders();
builder.Logging.AddSimpleConsole(options =>
{
    options.IncludeScopes = true; 
});

builder.Services.AddOpenApi("v1", options =>
{
    options.AddDocumentTransformer((document, context, cancellationToken) =>
    {
        var scheme = new Microsoft.OpenApi.Models.OpenApiSecurityScheme
        {
            Type = Microsoft.OpenApi.Models.SecuritySchemeType.Http,
            Scheme = "bearer",
            BearerFormat = "JWT",
            In = Microsoft.OpenApi.Models.ParameterLocation.Header,
            Description = "Enter your JWT token directly"
        };
        document.Components ??= new Microsoft.OpenApi.Models.OpenApiComponents();
        document.Components.SecuritySchemes.Add("Bearer", scheme);

        document.SecurityRequirements.Add(new Microsoft.OpenApi.Models.OpenApiSecurityRequirement
        {
            [new Microsoft.OpenApi.Models.OpenApiSecurityScheme
            {
                Reference = new Microsoft.OpenApi.Models.OpenApiReference
                {
                    Type = Microsoft.OpenApi.Models.ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            }] = Array.Empty<string>()
        });
        return Task.CompletedTask;
    });
});

builder.Services.AddControllers(options =>
{ options.Filters.Add<ValidateModelAttribute>(); })
.ConfigureApiBehaviorOptions(options =>
{
    options.SuppressModelStateInvalidFilter = true;

})
.AddDataAnnotationsLocalization(); ;
builder.Services.AddOpenApi();
// builder.Services.AddDbContext<AppDbContext>(options=>
// options.UseInMemoryDatabase("TraineeManagementDb"));
builder.Services.AddScoped<ITraineeRepository, TraineeRepository>();
builder.Services.AddScoped<IMentorRepository, MentorRepository>();
builder.Services.AddScoped<ILearningTaskRepository, LearningTaskRepository>();
builder.Services.AddScoped<ITaskAssignmentRepository, TaskAssignmentRepository>();
builder.Services.AddScoped<ISubmissionRepository, SubmissionRepository>();
builder.Services.AddScoped<IReviewRepository, ReviewRepository>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<ISubmissionFileRepository, SubmissionFileRepository>();
builder.Services.AddScoped<IProcessingJobRepository, ProcessingJobRepository>();

builder.Services.AddScoped<ITraineeService, TraineeService>();
builder.Services.AddScoped<IJWTService, JWTService>();
builder.Services.AddScoped<IMentorService, MentorService>();
builder.Services.AddScoped<ILearningTaskService, LearningTaskService>();
builder.Services.AddScoped<ITaskAssignmentService, TaskAssignmentService>();
builder.Services.AddScoped<ISubmissionService, SubmissionService>();
builder.Services.AddScoped<IReviewService, ReviewService>();
builder.Services.AddScoped<IFileStorageService,LocalFileStorageService>();
builder.Services.AddScoped<ICacheService, CacheService>();
builder.Services.AddScoped<IPublishRabbitMQService, PublishRabbitMQService>();
builder.Services.AddScoped<IProcessingJobService, ProcessingJobService>();


builder.Logging.ClearProviders();
builder.Logging.AddConsole();



var connectionString = builder.Configuration.GetConnectionString("DefaultConnection")
    ?? throw new InvalidOperationException("DefaultConnection string not found.");
var redisString = builder.Configuration.GetConnectionString("RedisConnection")
    ?? throw new InvalidOperationException("RedisConnection string not found.");
builder.Services.AddDbContext<AppDbContext>(options =>
       options.UseMySQL(connectionString));

builder.Services.AddStackExchangeRedisCache(options =>
{
    options.Configuration = redisString;
    options.InstanceName = "TraineeManagement";
});

builder.Services.AddHealthChecks()
    .AddCheck("self", () => HealthCheckResult.Healthy()) 
    .AddMySql(connectionString, name: "mysql", timeout: TimeSpan.FromSeconds(3))
    .AddRedis(redisString, name: "redis", timeout: TimeSpan.FromSeconds(3))      
    .AddRabbitMQ(sp => new ConnectionFactory
        {
            HostName = Config.RabbitHostName,
            Port = Config.RabbitPort,
            UserName = Config.RabbitUserName,
            Password = Config.RabbitPassword,
            VirtualHost = Config.RabbitVirtualHost
        }.CreateConnectionAsync(), name: "rabbitmq", timeout: TimeSpan.FromSeconds(3));
var app = builder.Build();

app.UseMiddleware<GlobalExceptionMiddleware>();

    app.MapOpenApi();
    app.UseSwaggerUi(options =>
    {
        options.DocumentPath = "/openapi/v1.json";
    });


app.UseCors("ReactClientPolicy");
app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
app.MapHealthChecks("/health/live", new Microsoft.AspNetCore.Diagnostics.HealthChecks.HealthCheckOptions
{
    Predicate = r => r.Name.Contains("self")
});
app.MapHealthChecks("/health/ready", new Microsoft.AspNetCore.Diagnostics.HealthChecks.HealthCheckOptions
{
    Predicate = _ => true,
    ResponseWriter = async (context, report) =>
    {
        context.Response.ContentType = "application/json";

        var response = new
        {
            status = report.Status.ToString(),
            checks = report.Entries.Select(entry => new
            {
                name = entry.Key,
                status = entry.Value.Status.ToString(),
                description = entry.Value.Description,
                duration = entry.Value.Duration.ToString(),
                error = entry.Value.Exception?.Message
            })
        };

        await context.Response.WriteAsync(JsonSerializer.Serialize(response));
    }
});

app.Run();
