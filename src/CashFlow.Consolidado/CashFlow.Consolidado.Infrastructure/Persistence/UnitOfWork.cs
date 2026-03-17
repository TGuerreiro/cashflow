using CashFlow.Shared.Domain.Interfaces;

namespace CashFlow.Consolidado.Infrastructure.Persistence;

public class UnitOfWork(ConsolidadoDbContext context) : IUnitOfWork
{
    private readonly ConsolidadoDbContext _context = context;

    public async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        return await _context.SaveChangesAsync(cancellationToken);
    }
}
