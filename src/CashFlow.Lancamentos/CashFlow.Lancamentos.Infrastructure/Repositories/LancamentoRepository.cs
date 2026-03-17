using CashFlow.Lancamentos.Domain.Entities;
using CashFlow.Lancamentos.Domain.Repositories;
using CashFlow.Lancamentos.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace CashFlow.Lancamentos.Infrastructure.Repositories;

public class LancamentoRepository(LancamentosDbContext context) : ILancamentoRepository
{
    private readonly LancamentosDbContext _context = context;

    public async Task<Lancamento?> GetByIdAsync(
        Guid id, CancellationToken cancellationToken = default)
    {
        return await _context.Lancamentos
            .FirstOrDefaultAsync(l => l.Id == id, cancellationToken);
    }

    public async Task AddAsync(
        Lancamento entity, CancellationToken cancellationToken = default)
    {
        await _context.Lancamentos.AddAsync(entity, cancellationToken);
    }

    public void Update(Lancamento entity)
    {
        _context.Lancamentos.Update(entity);
    }

    public void Remove(Lancamento entity)
    {
        _context.Lancamentos.Remove(entity);
    }

    public async Task<IReadOnlyList<Lancamento>> GetByDataAsync(
        DateOnly data, CancellationToken cancellationToken = default)
    {
        return await _context.Lancamentos
            .Where(l => l.Data == data)
            .OrderByDescending(l => l.CreatedAt)
            .ToListAsync(cancellationToken);
    }

    public async Task<IReadOnlyList<Lancamento>> GetByPeriodoAsync(
        DateOnly dataInicio, DateOnly dataFim, CancellationToken cancellationToken = default)
    {
        return await _context.Lancamentos
            .Where(l => l.Data >= dataInicio && l.Data <= dataFim)
            .OrderBy(l => l.Data)
            .ThenByDescending(l => l.CreatedAt)
            .ToListAsync(cancellationToken);
    }

    public async Task<IReadOnlyList<Lancamento>> GetAllPagedAsync(
        int page, int pageSize, CancellationToken cancellationToken = default)
    {
        return await _context.Lancamentos
            .OrderByDescending(l => l.Data)
            .ThenByDescending(l => l.CreatedAt)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(cancellationToken);
    }

    public async Task<int> CountAsync(CancellationToken cancellationToken = default)
    {
        return await _context.Lancamentos.CountAsync(cancellationToken);
    }
}
