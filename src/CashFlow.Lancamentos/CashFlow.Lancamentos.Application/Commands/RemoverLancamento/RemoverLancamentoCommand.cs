using CashFlow.Shared.Domain.Abstractions;
using MediatR;

namespace CashFlow.Lancamentos.Application.Commands.RemoverLancamento;

public record RemoverLancamentoCommand(Guid Id) : IRequest<Result>;
