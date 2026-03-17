using CashFlow.Consolidado.Domain.Entities;
using CashFlow.Consolidado.Domain.Repositories;
using CashFlow.Consolidado.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace CashFlow.Consolidado.Infrastructure.Repositories;

public class ConsolidadoDiarioRepository(ConsolidadoDbContext context) : IConsolidadoDiarioRepository
{
    private readonly ConsolidadoDbContext _context = context;

    public async Task<ConsolidadoDiario?> GetByIdAsync(
        Guid id, CancellationToken cancellationToken = default)
    {
        return await _context.Consolidados
            .FirstOrDefaultAsync(c => c.Id == id, cancellationToken);
    }

    public async Task AddAsync(
        ConsolidadoDiario entity, CancellationToken cancellationToken = default)
    {
        await _context.Consolidados.AddAsync(entity, cancellationToken);
    }

    public void Update(ConsolidadoDiario entity)
    {
        _context.Consolidados.Update(entity);
    }

    public void Remove(ConsolidadoDiario entity)
    {
        _context.Consolidados.Remove(entity);
    }

    public async Task<ConsolidadoDiario?> GetByDataAsync(
        DateOnly data, CancellationToken cancellationToken = default)
    {
        return await _context.Consolidados
            .FirstOrDefaultAsync(c => c.Data == data, cancellationToken);
    }

    public async Task<IReadOnlyList<ConsolidadoDiario>> GetByPeriodoAsync(
        DateOnly dataInicio, DateOnly dataFim, CancellationToken cancellationToken = default)
    {
        return await _context.Consolidados
            .Where(c => c.Data >= dataInicio && c.Data <= dataFim)
            .OrderBy(c => c.Data)
            .ToListAsync(cancellationToken);
    }
}
