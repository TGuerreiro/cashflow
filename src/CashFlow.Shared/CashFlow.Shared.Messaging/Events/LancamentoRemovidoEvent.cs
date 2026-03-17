using CashFlow.Shared.Domain.Enums;

namespace CashFlow.Shared.Messaging.Events;

public record LancamentoRemovidoEvent : IIntegrationEvent
{
    public Guid EventId { get; init; } = Guid.NewGuid();
    public DateTime OccurredAt { get; init; } = DateTime.UtcNow;

    public required Guid LancamentoId { get; init; }
    public required DateOnly Data { get; init; }
    public required decimal Valor { get; init; }
    public required TipoLancamento Tipo { get; init; }
}
