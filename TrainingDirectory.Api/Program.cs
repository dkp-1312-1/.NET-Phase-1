using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;

namespace TrainingDirectory.Api
{
    public class Program
    {
        public static void Main(string[] args)
        {
            WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

TrainingDirectory.Api.Utils.Config.Initialize(builder.Configuration);

builder.Services.AddControllers();

builder.Logging.ClearProviders();
Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Information()
    .WriteTo.Console()
    .WriteTo.Logger(lc => lc
        .Filter.ByExcluding(logEvent =>
            logEvent.Properties.TryGetValue("SourceContext", out var value) &&
            (value.ToString().Contains("Microsoft") || value.ToString().Contains("System")))
        .WriteTo.File(
            path: "logs/directory-.txt",
            rollingInterval: RollingInterval.Day,
            outputTemplate: "{Timestamp:yyyy-MM-dd HH:mm:ss.fff zzz} [{Level:u3}] {Message:lj}{NewLine}"))        
    .CreateLogger();

builder.Host.UseSerilog();


WebApplication app = builder.Build();

app.UseRouting();

app.UseAuthorization();

app.MapControllers();

            app.Run();
        }
    }
}
