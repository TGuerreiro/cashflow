using CashFlow.Lancamentos.Application.DTOs;
using CashFlow.Shared.Domain.Abstractions;
using MediatR;

namespace CashFlow.Lancamentos.Application.Queries.ObterLancamentoPorId;

public record ObterLancamentoPorIdQuery(Guid Id) : IRequest<Result<LancamentoResponse>>;
