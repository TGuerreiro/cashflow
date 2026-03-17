using CashFlow.Lancamentos.Application.DTOs;
using CashFlow.Lancamentos.Domain.Errors;
using CashFlow.Lancamentos.Domain.Repositories;
using CashFlow.Shared.Domain.Abstractions;
using CashFlow.Shared.Domain.Interfaces;
using MediatR;

namespace CashFlow.Lancamentos.Application.Commands.AtualizarLancamento;

public  class AtualizarLancamentoHandler(
    ILancamentoRepository repository,
    IUnitOfWork unitOfWork)
        : IRequestHandler<AtualizarLancamentoCommand, Result<LancamentoResponse>>
{
    private readonly ILancamentoRepository _repository = repository;
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    public async Task<Result<LancamentoResponse>> Handle(
        AtualizarLancamentoCommand request,
        CancellationToken cancellationToken)
    {
        var lancamento = await _repository.GetByIdAsync(request.Id, cancellationToken);

        if (lancamento is null)
            return Result<LancamentoResponse>.Failure(
                LancamentoErrors.NaoEncontrado(request.Id));

        var result = lancamento.Atualizar(
            request.Data,
            request.Valor,
            request.Tipo,
            request.Descricao);

        if (result.IsFailure)
            return Result<LancamentoResponse>.Failure(result.Error);

        _repository.Update(lancamento);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result<LancamentoResponse>.Success(lancamento.ToResponse());
    }
}
