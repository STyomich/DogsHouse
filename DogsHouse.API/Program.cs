using DogsHouse.API.Extensions;
using DogsHouse.Application.Configuration;
using DogsHouse.Infrastructure.MSSQL.Configuration;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddApiServices();
builder.Services.AddApplicationServices();
builder.Services.AddInfrastructureMSSSQLServices();
builder.Services.AddPersistenceServices(builder.Configuration);

var app = builder.Build();

// Add Swagger middleware
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "DogHouse API v1");
        c.RoutePrefix = "swagger";
    });
}

app.UseCustomMiddlewares();

await app.RunAsync();