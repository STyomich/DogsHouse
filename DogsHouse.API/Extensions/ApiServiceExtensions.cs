using Microsoft.OpenApi.Models;

namespace DogsHouse.API.Extensions;

public static class ApiServiceExtensions
{
    public static IServiceCollection AddApiServices(this IServiceCollection services)
    {
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen(c =>
        {
            c.SwaggerDoc("v1", new OpenApiInfo
            {
                Title = "DogHouse API",
                Version = "v1"
            });
        });
        return services;
    }
}
