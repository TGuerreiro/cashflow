using CashFlow.Lancamentos.Application.Commands.RemoverLancamento;
using CashFlow.Lancamentos.Domain.Entities;
using CashFlow.Lancamentos.Domain.Repositories;
using CashFlow.Shared.Domain.Enums;
using CashFlow.Shared.Domain.Interfaces;
using FluentAssertions;
using NSubstitute;

namespace CashFlow.Lancamentos.UnitTests;

public class RemoverLancamentoHandlerTests
{
    private readonly ILancamentoRepository _repository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly RemoverLancamentoHandler _handler;

    public RemoverLancamentoHandlerTests()
    {
        _repository = Substitute.For<ILancamentoRepository>();
        _unitOfWork = Substitute.For<IUnitOfWork>();
        _handler = new RemoverLancamentoHandler(_repository, _unitOfWork);
    }

    [Fact]
    public async Task Handle_QuandoLancamentoExiste_DeveRemoverESalvar()
    {
        // Arrange
        var id = Guid.NewGuid();
        var lancamento = Lancamento.Criar(DateOnly.FromDateTime(DateTime.Now), 100m, TipoLancamento.Credito, "Teste").Value;
        _repository.GetByIdAsync(id, Arg.Any<CancellationToken>()).Returns(lancamento);

        var command = new RemoverLancamentoCommand(id);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeTrue();
        _repository.Received(1).Remove(lancamento);
        await _unitOfWork.Received(1).SaveChangesAsync(Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task Handle_QuandoLancamentoNaoExiste_DeveRetornarFalha()
    {
        // Arrange
        var id = Guid.NewGuid();
        _repository.GetByIdAsync(id, Arg.Any<CancellationToken>()).Returns((Lancamento)null!);

        var command = new RemoverLancamentoCommand(id);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.IsFailure.Should().BeTrue();
        await _unitOfWork.DidNotReceive().SaveChangesAsync(Arg.Any<CancellationToken>());
    }
}
