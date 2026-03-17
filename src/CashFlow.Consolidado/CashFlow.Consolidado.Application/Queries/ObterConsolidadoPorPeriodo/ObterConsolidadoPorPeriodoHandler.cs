using CashFlow.Consolidado.Application.DTOs;
using CashFlow.Consolidado.Domain.Errors;
using CashFlow.Consolidado.Domain.Repositories;
using CashFlow.Shared.Domain.Abstractions;
using MediatR;

namespace CashFlow.Consolidado.Application.Queries.ObterConsolidadoPorPeriodo;

public class ObterConsolidadoPorPeriodoHandler(IConsolidadoDiarioRepository repository)
        : IRequestHandler<ObterConsolidadoPorPeriodoQuery, Result<IReadOnlyList<ConsolidadoResponse>>>
{
    private readonly IConsolidadoDiarioRepository _repository = repository;

    public async Task<Result<IReadOnlyList<ConsolidadoResponse>>> Handle(
        ObterConsolidadoPorPeriodoQuery request,
        CancellationToken cancellationToken)
    {
        if (request.DataInicio > request.DataFim)
            return Result<IReadOnlyList<ConsolidadoResponse>>.Failure(
                ConsolidadoErrors.PeriodoInvalido);

        var consolidados = await _repository.GetByPeriodoAsync(
            request.DataInicio, request.DataFim, cancellationToken);

        return Result<IReadOnlyList<ConsolidadoResponse>>.Success(
            consolidados.ToResponseList());
    }
}
