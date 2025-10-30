using DogsHouse.API.Extensions;
using DogsHouse.Application.Configuration;
using DogsHouse.Infrastructure.MSSQL.Configuration;

var builder = WebApplication.CreateBuilder(args);

var logsDirectory = Path.Combine(Directory.GetCurrentDirectory(), "Logs");

// Adding services of different layers.
builder.Services.AddApiServices();
builder.Services.AddApplicationServices();
builder.Services.AddInfrastructureMSSSQLServices();
builder.Services.AddPersistenceServices(builder.Configuration);

// Setting providers.
builder.Logging.ClearProviders();
builder.Logging.AddConsole();
builder.Logging.AddProvider(new DogsHouse.API.Logging.FileLoggerProvider(logsDirectory));

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    // Add Swagger middleware.
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "DogHouse API v1");
        c.RoutePrefix = "swagger";
    });
}

// Using custom middlewares.
app.UseCustomMiddlewares();

// Enable rate limiting for requests.
app.UseRateLimiter();

app.MapControllers();

await app.RunAsync();