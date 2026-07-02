using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);


builder.Services.AddControllers();
builder.Services.AddLogging();

WebApplication app = builder.Build();

app.UseRouting();

app.UseAuthorization();

app.MapControllers();

app.Run();
