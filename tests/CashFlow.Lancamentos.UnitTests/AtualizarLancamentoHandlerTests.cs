using CashFlow.Lancamentos.Application.Commands.AtualizarLancamento;
using CashFlow.Lancamentos.Domain.Entities;
using CashFlow.Lancamentos.Domain.Repositories;
using CashFlow.Shared.Domain.Enums;
using CashFlow.Shared.Domain.Interfaces;
using FluentAssertions;
using NSubstitute;

namespace CashFlow.Lancamentos.UnitTests;

public class AtualizarLancamentoHandlerTests
{
    private readonly ILancamentoRepository _repository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly AtualizarLancamentoHandler _handler;

    public AtualizarLancamentoHandlerTests()
    {
        _repository = Substitute.For<ILancamentoRepository>();
        _unitOfWork = Substitute.For<IUnitOfWork>();
        _handler = new AtualizarLancamentoHandler(_repository, _unitOfWork);
    }

    [Fact]
    public async Task Handle_QuandoLancamentoExisteEDadosValidos_DeveAtualizarESalvar()
    {
        // Arrange
        var id = Guid.NewGuid();
        var lancamento = Lancamento.Criar(DateOnly.FromDateTime(DateTime.Now), 100m, TipoLancamento.Credito, "Teste").Value;
        _repository.GetByIdAsync(id, Arg.Any<CancellationToken>()).Returns(lancamento);

        var command = new AtualizarLancamentoCommand(
            id,
            DateOnly.FromDateTime(DateTime.Now.AddDays(1)),
            200m,
            TipoLancamento.Debito,
            "Atualizado");

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeTrue();
        lancamento.Valor.Amount.Should().Be(200m);
        lancamento.Descricao.Should().Be("Atualizado");

        await _unitOfWork.Received(1).SaveChangesAsync(Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task Handle_QuandoLancamentoNaoExiste_DeveRetornarFalha()
    {
        // Arrange
        var id = Guid.NewGuid();
        _repository.GetByIdAsync(id, Arg.Any<CancellationToken>()).Returns((Lancamento)null!);

        var command = new AtualizarLancamentoCommand(id, DateOnly.FromDateTime(DateTime.Now), 100m, TipoLancamento.Credito, "Teste");

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.IsFailure.Should().BeTrue();
        await _unitOfWork.DidNotReceive().SaveChangesAsync(Arg.Any<CancellationToken>());
    }
}
