using DogsHouse.Application.Models;
using Microsoft.EntityFrameworkCore;

namespace DogsHouse.Infrastructure.MSSQL.DbContext;

public class DogsHouseDbContext(DbContextOptions<DogsHouseDbContext> options) : Microsoft.EntityFrameworkCore.DbContext(options)
{
    public DbSet<Dog> Dogs { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.ApplyConfigurationsFromAssembly(typeof(DogsHouseDbContext).Assembly);
    }
}
