using DogsHouse.Application.Interfaces.Repositories;
using DogsHouse.Application.Models;
using DogsHouse.Infrastructure.MSSQL.DbContext;
using Microsoft.EntityFrameworkCore;

namespace DogsHouse.Infrastructure.MSSQL.Repositories;

public class DogsRepository(DogsHouseDbContext context) : IDogsRepository
{
    private readonly DbSet<Dog> _dbSet = context.Set<Dog>();

    public async Task AddAsync(Dog dog, CancellationToken cancellationToken)
    {
        await _dbSet.AddAsync(dog, cancellationToken);
    }

    public async Task<ICollection<Dog>> GetAllDogsAsync(CancellationToken cancellationToken)
    {
        return await _dbSet.ToListAsync(cancellationToken);
    }

    public Task<Dog> GetDogByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public async Task<bool> ExistsByNameAsync(string name, CancellationToken cancellationToken) => await _dbSet.AnyAsync(d => d.Name == name, cancellationToken);

    public IQueryable<Dog> Query()
    {
        return _dbSet.AsQueryable();
    }
}
