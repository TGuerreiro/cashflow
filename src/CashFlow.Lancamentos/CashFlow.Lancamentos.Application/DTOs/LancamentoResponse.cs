using CashFlow.Shared.Domain.Enums;

namespace CashFlow.Lancamentos.Application.DTOs;

public record LancamentoResponse(
    Guid Id,
    DateOnly Data,
    decimal Valor,
    TipoLancamento Tipo,
    string TipoDescricao,
    string Descricao,
    DateTime CreatedAt,
    DateTime? UpdatedAt);
