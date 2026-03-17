using CashFlow.Shared.Domain.Enums;

namespace CashFlow.Consolidado.Application.Services;

public interface IConsolidacaoService
{
    Task AplicarLancamentoAsync(
        DateOnly data,
        decimal valor,
        TipoLancamento tipo,
        CancellationToken cancellationToken = default);

    Task ReverterLancamentoAsync(
        DateOnly data,
        decimal valor,
        TipoLancamento tipo,
        CancellationToken cancellationToken = default);
}
