using CashFlow.Lancamentos.Domain.Entities;

namespace CashFlow.Lancamentos.Application.DTOs;

public static class LancamentoMapper
{
    public static LancamentoResponse ToResponse(this Lancamento lancamento)
    {
        return new LancamentoResponse(
            lancamento.Id,
            lancamento.Data,
            lancamento.Valor.Amount,
            lancamento.Tipo,
            lancamento.Tipo.ToString(),
            lancamento.Descricao,
            lancamento.CreatedAt,
            lancamento.UpdatedAt);
    }

    public static IReadOnlyList<LancamentoResponse> ToResponseList(
        this IEnumerable<Lancamento> lancamentos)
    {
        return lancamentos.Select(l => l.ToResponse()).ToList().AsReadOnly();
    }
}