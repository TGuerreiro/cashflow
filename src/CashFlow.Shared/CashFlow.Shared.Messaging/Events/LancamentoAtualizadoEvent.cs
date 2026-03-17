using CashFlow.Shared.Domain.Enums;

namespace CashFlow.Shared.Messaging.Events;

public record LancamentoAtualizadoEvent : IIntegrationEvent
{
    public Guid EventId { get; init; } = Guid.NewGuid();
    public DateTime OccurredAt { get; init; } = DateTime.UtcNow;

    public required Guid LancamentoId { get; init; }
    public required DateOnly Data { get; init; }
    public required decimal ValorAnterior { get; init; }
    public required decimal ValorNovo { get; init; }
    public required TipoLancamento TipoAnterior { get; init; }
    public required TipoLancamento TipoNovo { get; init; }
}
