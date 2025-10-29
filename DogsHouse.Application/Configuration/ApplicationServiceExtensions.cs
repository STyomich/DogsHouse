using DogsHouse.Application.Interfaces;
using DogsHouse.Application.Services;
using Microsoft.Extensions.DependencyInjection;

namespace DogsHouse.Application.Configuration;

public static class ApplicationServiceExtensions
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        services.AddScoped<IDogsService, DogsService>();
        return services;
    }
}
