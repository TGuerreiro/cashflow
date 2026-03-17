using CashFlow.Consolidado.Domain.Entities;
using CashFlow.Shared.Domain.Interfaces;

namespace CashFlow.Consolidado.Domain.Repositories;

public interface IConsolidadoDiarioRepository : IRepository<ConsolidadoDiario>
{
    Task<ConsolidadoDiario?> GetByDataAsync(
        DateOnly data,
        CancellationToken cancellationToken = default);

    Task<IReadOnlyList<ConsolidadoDiario>> GetByPeriodoAsync(
        DateOnly dataInicio,
        DateOnly dataFim,
        CancellationToken cancellationToken = default);
}
