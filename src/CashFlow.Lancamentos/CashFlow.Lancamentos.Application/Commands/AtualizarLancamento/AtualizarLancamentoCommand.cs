using CashFlow.Lancamentos.Application.DTOs;
using CashFlow.Shared.Domain.Abstractions;
using CashFlow.Shared.Domain.Enums;
using MediatR;

namespace CashFlow.Lancamentos.Application.Commands.AtualizarLancamento;

public  record AtualizarLancamentoCommand(
    Guid Id,
    DateOnly Data,
    decimal Valor,
    TipoLancamento Tipo,
    string Descricao) : IRequest<Result<LancamentoResponse>>;
