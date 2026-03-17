using CashFlow.Lancamentos.Application.DTOs;
using CashFlow.Shared.Domain.Abstractions;
using MediatR;

namespace CashFlow.Lancamentos.Application.Queries.ObterLancamentosPaginado;

public record ObterLancamentosPaginadoQuery(
    int Page = 1,
    int PageSize = 20) : IRequest<Result<PagedResponse<LancamentoResponse>>>;
