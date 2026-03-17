using CashFlow.Consolidado.Domain.Entities;

namespace CashFlow.Consolidado.Application.DTOs;

public static class ConsolidadoMapper
{
    public static ConsolidadoResponse ToResponse(this ConsolidadoDiario consolidado)
    {
        return new ConsolidadoResponse(
            consolidado.Id,
            consolidado.Data,
            consolidado.TotalCreditos,
            consolidado.TotalDebitos,
            consolidado.Saldo,
            consolidado.QuantidadeLancamentos,
            consolidado.CreatedAt,
            consolidado.UpdatedAt);
    }

    public static IReadOnlyList<ConsolidadoResponse> ToResponseList(
        this IEnumerable<ConsolidadoDiario> consolidados)
    {
        return consolidados.Select(c => c.ToResponse()).ToList().AsReadOnly();
    }
}
