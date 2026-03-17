using CashFlow.Shared.Domain.Abstractions;
using CashFlow.Shared.Domain.Enums;

namespace CashFlow.Lancamentos.Domain.Events;

public record LancamentoRegistradoDomainEvent(
    Guid LancamentoId,
    DateOnly Data,
    decimal Valor,
    TipoLancamento Tipo,
    string Descricao) : DomainEvent;

public record LancamentoAtualizadoDomainEvent(
    Guid LancamentoId,
    DateOnly Data,
    decimal ValorAnterior,
    decimal ValorNovo,
    TipoLancamento TipoAnterior,
    TipoLancamento TipoNovo) : DomainEvent;

public record LancamentoRemovidoDomainEvent(
    Guid LancamentoId,
    DateOnly Data,
    decimal Valor,
    TipoLancamento Tipo) : DomainEvent;
