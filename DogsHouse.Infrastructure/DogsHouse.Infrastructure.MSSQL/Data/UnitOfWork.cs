using DogsHouse.Application.Interfaces.Repositories;
using DogsHouse.Infrastructure.MSSQL.DbContext;
using DogsHouse.Infrastructure.MSSQL.Repositories;

namespace DogsHouse.Infrastructure.MSSQL.Data;

public class UnitOfWork(DogsHouseDbContext context) : IUnitOfWork
{
    private readonly DogsHouseDbContext _context = context;

    private IDogsRepository? _dogsRepository;

    public IDogsRepository DogsRepository
    {
        get
        {
            _dogsRepository ??= new DogsRepository(_context);

            return _dogsRepository;
        }
    }

    public Task<int> SaveChangesAsync(CancellationToken cancellationToken) => _context.SaveChangesAsync(cancellationToken);
}
