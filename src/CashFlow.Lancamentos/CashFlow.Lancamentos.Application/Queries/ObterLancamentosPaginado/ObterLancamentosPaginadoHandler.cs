using CashFlow.Lancamentos.Application.DTOs;
using CashFlow.Lancamentos.Domain.Repositories;
using CashFlow.Shared.Domain.Abstractions;
using MediatR;

namespace CashFlow.Lancamentos.Application.Queries.ObterLancamentosPaginado;

public class ObterLancamentosPaginadoHandler(ILancamentoRepository repository)
        : IRequestHandler<ObterLancamentosPaginadoQuery, Result<PagedResponse<LancamentoResponse>>>
{
    private readonly ILancamentoRepository _repository = repository;

    public async Task<Result<PagedResponse<LancamentoResponse>>> Handle(
        ObterLancamentosPaginadoQuery request,
        CancellationToken cancellationToken)
    {
        var lancamentos = await _repository.GetAllPagedAsync(
            request.Page,
            request.PageSize,
            cancellationToken);

        var totalCount = await _repository.CountAsync(cancellationToken);

        var response = new PagedResponse<LancamentoResponse>(
            lancamentos.ToResponseList(),
            request.Page,
            request.PageSize,
            totalCount);

        return Result<PagedResponse<LancamentoResponse>>.Success(response);
    }
}
