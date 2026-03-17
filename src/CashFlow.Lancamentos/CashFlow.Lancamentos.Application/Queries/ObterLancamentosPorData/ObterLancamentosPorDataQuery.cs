using CashFlow.Lancamentos.Application.DTOs;
using CashFlow.Shared.Domain.Abstractions;
using MediatR;

namespace CashFlow.Lancamentos.Application.Queries.ObterLancamentosPorData;

public record ObterLancamentosPorDataQuery(DateOnly Data) : IRequest<Result<IReadOnlyList<LancamentoResponse>>>;
