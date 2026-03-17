using CashFlow.Lancamentos.Domain.Entities;
using CashFlow.Shared.Domain.Interfaces;

namespace CashFlow.Lancamentos.Domain.Repositories;

public interface ILancamentoRepository : IRepository<Lancamento>
{
    Task<IReadOnlyList<Lancamento>> GetByDataAsync(
        DateOnly data,
        CancellationToken cancellationToken = default);

    Task<IReadOnlyList<Lancamento>> GetByPeriodoAsync(
        DateOnly dataInicio,
        DateOnly dataFim,
        CancellationToken cancellationToken = default);

    Task<IReadOnlyList<Lancamento>> GetAllPagedAsync(
        int page,
        int pageSize,
        CancellationToken cancellationToken = default);

    Task<int> CountAsync(CancellationToken cancellationToken = default);
}
