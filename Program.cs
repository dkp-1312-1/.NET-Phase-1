using System.Reflection.Metadata;
using TraineeManagement1.Services;
using TraineeManagement1.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers().ConfigureApiBehaviorOptions(options =>
    {
        options.InvalidModelStateResponseFactory = context =>
        {
            // Extract the errors from the ModelState
            var errors = context.ModelState
                .Where(e => e.Value.Errors.Count > 0)
                .Select(e => new
                {
                    Field = e.Key,
                    Message = e.Value.Errors.First().ErrorMessage
                }).ToList();

            // Create your own custom response payload shape
            var customResponse = new
            {
                Errors = errors,
                Success=false
            };

            // Return your custom structure as a Bad Request (400)
            return new BadRequestObjectResult(customResponse);
        };
    });
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();
builder.Services.AddDbContext<AppDbContext>(options=>
options.UseInMemoryDatabase("TraineeManagementDb"));
builder.Services.AddScoped<ITraineeService, TraineeService>();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();


    // Serves the interactive Swagger UI web page
    app.UseSwaggerUi(options =>
    {
        options.DocumentPath = "/openapi/v1.json";
    });
}


app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
