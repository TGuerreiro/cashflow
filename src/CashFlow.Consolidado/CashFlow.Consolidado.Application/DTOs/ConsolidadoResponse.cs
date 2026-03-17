namespace CashFlow.Consolidado.Application.DTOs;

public record ConsolidadoResponse(
    Guid Id,
    DateOnly Data,
    decimal TotalCreditos,
    decimal TotalDebitos,
    decimal Saldo,
    int QuantidadeLancamentos,
    DateTime CreatedAt,
    DateTime? UpdatedAt);