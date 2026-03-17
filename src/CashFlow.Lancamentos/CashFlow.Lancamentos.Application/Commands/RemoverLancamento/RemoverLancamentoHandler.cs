using CashFlow.Lancamentos.Domain.Errors;
using CashFlow.Lancamentos.Domain.Repositories;
using CashFlow.Shared.Domain.Abstractions;
using CashFlow.Shared.Domain.Interfaces;
using MediatR;

namespace CashFlow.Lancamentos.Application.Commands.RemoverLancamento;

public class RemoverLancamentoHandler(
    ILancamentoRepository repository,
    IUnitOfWork unitOfWork)
        : IRequestHandler<RemoverLancamentoCommand, Result>
{
    private readonly ILancamentoRepository _repository = repository;
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    public async Task<Result> Handle(
        RemoverLancamentoCommand request,
        CancellationToken cancellationToken)
    {
        var lancamento = await _repository.GetByIdAsync(request.Id, cancellationToken);

        if (lancamento is null)
            return Result.Failure(LancamentoErrors.NaoEncontrado(request.Id));

        lancamento.MarcarParaRemocao();
        _repository.Remove(lancamento);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}
