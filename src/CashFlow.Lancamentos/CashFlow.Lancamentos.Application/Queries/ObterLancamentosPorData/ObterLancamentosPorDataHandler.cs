using CashFlow.Lancamentos.Application.DTOs;
using CashFlow.Lancamentos.Domain.Repositories;
using CashFlow.Shared.Domain.Abstractions;
using MediatR;

namespace CashFlow.Lancamentos.Application.Queries.ObterLancamentosPorData;

public class ObterLancamentosPorDataHandler(ILancamentoRepository repository)
        : IRequestHandler<ObterLancamentosPorDataQuery, Result<IReadOnlyList<LancamentoResponse>>>
{
    private readonly ILancamentoRepository _repository = repository;

    public async Task<Result<IReadOnlyList<LancamentoResponse>>> Handle(
        ObterLancamentosPorDataQuery request,
        CancellationToken cancellationToken)
    {
        var lancamentos = await _repository.GetByDataAsync(request.Data, cancellationToken);
        return Result<IReadOnlyList<LancamentoResponse>>.Success(lancamentos.ToResponseList());
    }
}
