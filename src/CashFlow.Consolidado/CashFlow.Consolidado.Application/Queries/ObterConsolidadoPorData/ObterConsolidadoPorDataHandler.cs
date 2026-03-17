using CashFlow.Consolidado.Application.DTOs;
using CashFlow.Consolidado.Domain.Repositories;
using CashFlow.Shared.Domain.Abstractions;
using MediatR;

namespace CashFlow.Consolidado.Application.Queries.ObterConsolidadoPorData;

public class ObterConsolidadoPorDataHandler(IConsolidadoDiarioRepository repository)
        : IRequestHandler<ObterConsolidadoPorDataQuery, Result<ConsolidadoResponse>>
{
    private readonly IConsolidadoDiarioRepository _repository = repository;

    public async Task<Result<ConsolidadoResponse>> Handle(
        ObterConsolidadoPorDataQuery request,
        CancellationToken cancellationToken)
    {
        var consolidado = await _repository.GetByDataAsync(request.Data, cancellationToken);

        if (consolidado is null)
        {
            // Se não houver consolidado para a data, retorna um objeto zerado em vez de erro, pois o saldo é implicitamente zero.
            return Result<ConsolidadoResponse>.Success(new ConsolidadoResponse(
                Guid.Empty,
                request.Data,
                0,
                0,
                0,
                0,
                DateTime.UtcNow,
                null));
        }

        return Result<ConsolidadoResponse>.Success(consolidado.ToResponse());
    }
}