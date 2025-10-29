using DogsHouse.Infrastructure.MSSQL.DbContext;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace DogsHouse.Infrastructure.MSSQL.Configuration;

public static class PersistenceServiceConfiguration
{
    public static IServiceCollection AddPersistenceServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<DogsHouseDbContext>(opt =>
        {
            opt.UseSqlServer(configuration.GetConnectionString("DefaultConnection"));
        });
        services.AddScoped<DogsHouseDbContext>();
        return services;
    }
}
