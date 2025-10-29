using DogsHouse.Application.Interfaces.Repositories;
using DogsHouse.Infrastructure.MSSQL.Data;
using DogsHouse.Infrastructure.MSSQL.Repositories;
using Microsoft.Extensions.DependencyInjection;

namespace DogsHouse.Infrastructure.MSSQL.Configuration;

public static class InfrastructureMSSQLServiceExtensions
{
    /// <summary>
    /// Adds implemented services to service collection of application.
    /// </summary>
    public static IServiceCollection AddInfrastructureMSSSQLServices(this IServiceCollection services)
    {
        services.AddScoped<IDogsRepository, DogsRepository>();
        services.AddScoped<IUnitOfWork, UnitOfWork>();
        return services;
    }
}
