using CashFlow.Lancamentos.Application.DTOs;
using CashFlow.Lancamentos.Domain.Errors;
using CashFlow.Lancamentos.Domain.Repositories;
using CashFlow.Shared.Domain.Abstractions;
using MediatR;

namespace CashFlow.Lancamentos.Application.Queries.ObterLancamentoPorId;

public class ObterLancamentoPorIdHandler(ILancamentoRepository repository)
        : IRequestHandler<ObterLancamentoPorIdQuery, Result<LancamentoResponse>>
{
    private readonly ILancamentoRepository _repository = repository;

    public async Task<Result<LancamentoResponse>> Handle(
        ObterLancamentoPorIdQuery request,
        CancellationToken cancellationToken)
    {
        var lancamento = await _repository.GetByIdAsync(request.Id, cancellationToken);

        if (lancamento is null)
            return Result<LancamentoResponse>.Failure(
                LancamentoErrors.NaoEncontrado(request.Id));

        return Result<LancamentoResponse>.Success(lancamento.ToResponse());
    }
}
