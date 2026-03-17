using CashFlow.Lancamentos.Application.DTOs;
using CashFlow.Lancamentos.Domain.Entities;
using CashFlow.Lancamentos.Domain.Repositories;
using CashFlow.Shared.Domain.Abstractions;
using CashFlow.Shared.Domain.Interfaces;
using MediatR;

namespace CashFlow.Lancamentos.Application.Commands.CriarLancamento;

public class CriarLancamentoHandler(
    ILancamentoRepository repository,
    IUnitOfWork unitOfWork)
        : IRequestHandler<CriarLancamentoCommand, Result<LancamentoResponse>>
{
    private readonly ILancamentoRepository _repository = repository;
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    public async Task<Result<LancamentoResponse>> Handle(
        CriarLancamentoCommand request,
        CancellationToken cancellationToken)
    {
        var result = Lancamento.Criar(
            request.Data,
            request.Valor,
            request.Tipo,
            request.Descricao);

        if (result.IsFailure)
            return Result<LancamentoResponse>.Failure(result.Error);

        var lancamento = result.Value;

        await _repository.AddAsync(lancamento, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result<LancamentoResponse>.Success(lancamento.ToResponse());
    }
}
