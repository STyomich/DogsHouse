using System.Threading.RateLimiting;
using Microsoft.AspNetCore.RateLimiting;
using Microsoft.OpenApi.Models;

namespace DogsHouse.API.Extensions;

public static class ApiServiceExtensions
{
    public static IServiceCollection AddApiServices(this IServiceCollection services)
    {
        services.AddControllers();
        services.AddEndpointsApiExplorer();
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen(c =>
        {
            c.SwaggerDoc("v1", new OpenApiInfo
            {
                Title = "DogHouse API",
                Version = "v1"
            });
        });
        services.AddRateLimiter(options =>
        {
            options.AddFixedWindowLimiter("FixedWindowPolicy", limiterOptions =>
            {
                limiterOptions.PermitLimit = 10; // maximum 10 requests
                limiterOptions.Window = TimeSpan.FromSeconds(1);
                limiterOptions.QueueProcessingOrder = QueueProcessingOrder.OldestFirst;
                limiterOptions.QueueLimit = 0; // reject immediately when over limit
            });
        });
        return services;
    }
}
