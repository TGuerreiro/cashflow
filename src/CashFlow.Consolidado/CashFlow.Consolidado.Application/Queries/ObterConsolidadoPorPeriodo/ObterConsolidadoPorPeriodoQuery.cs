using CashFlow.Consolidado.Application.DTOs;
using CashFlow.Shared.Domain.Abstractions;
using MediatR;

namespace CashFlow.Consolidado.Application.Queries.ObterConsolidadoPorPeriodo;

public record ObterConsolidadoPorPeriodoQuery(
    DateOnly DataInicio,
    DateOnly DataFim) : IRequest<Result<IReadOnlyList<ConsolidadoResponse>>>;
