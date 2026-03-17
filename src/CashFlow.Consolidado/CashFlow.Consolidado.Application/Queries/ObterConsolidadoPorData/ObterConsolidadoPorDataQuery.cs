using CashFlow.Consolidado.Application.DTOs;
using CashFlow.Shared.Domain.Abstractions;
using MediatR;

namespace CashFlow.Consolidado.Application.Queries.ObterConsolidadoPorData;

public record ObterConsolidadoPorDataQuery(DateOnly Data)
    : IRequest<Result<ConsolidadoResponse>>;
