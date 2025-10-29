using DogsHouse.Application.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DogsHouse.Infrastructure.MSSQL.Data.Configurations;

/// <summary>
/// Configuration for Dog entity.
/// </summary>
public class DogConfiguration : IEntityTypeConfiguration<Dog>
{
    public void Configure(EntityTypeBuilder<Dog> builder)
    {
        builder
            .HasKey(d => d.Id);
    }
}
