using CashFlow.Lancamentos.Application.Commands.CriarLancamento;
using CashFlow.Lancamentos.Domain.Entities;
using CashFlow.Lancamentos.Domain.Repositories;
using CashFlow.Shared.Domain.Enums;
using CashFlow.Shared.Domain.Interfaces;
using FluentAssertions;
using NSubstitute;

namespace CashFlow.Lancamentos.UnitTests;

public class CriarLancamentoHandlerTests
{
    private readonly ILancamentoRepository _repository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly CriarLancamentoHandler _handler;

    public CriarLancamentoHandlerTests()
    {
        _repository = Substitute.For<ILancamentoRepository>();
        _unitOfWork = Substitute.For<IUnitOfWork>();
        _handler = new CriarLancamentoHandler(_repository, _unitOfWork);
    }

    [Fact]
    public async Task Handle_ComComandoValido_DeveAdicionarLancamentoESalvar()
    {
        // Arrange
        var command = new CriarLancamentoCommand(
            DateOnly.FromDateTime(DateTime.Now),
            100.50m,
            TipoLancamento.Credito,
            "Venda de Produto");

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Value.Should().NotBeNull();
        result.Value.Valor.Should().Be(command.Valor);
        result.Value.Tipo.Should().Be(command.Tipo);
        result.Value.Descricao.Should().Be(command.Descricao);

        await _repository.Received(1).AddAsync(Arg.Any<Lancamento>(), Arg.Any<CancellationToken>());
        await _unitOfWork.Received(1).SaveChangesAsync(Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task Handle_ComDadosInvalidos_DeveRetornarFalhaENaoSalvar()
    {
        // Arrange
        var command = new CriarLancamentoCommand(
            DateOnly.FromDateTime(DateTime.Now),
            -10.00m, // Valor inválido
            TipoLancamento.Credito,
            "Teste");

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.IsFailure.Should().BeTrue();
        result.Error.Code.Should().Be("Money.InvalidAmount");

        await _repository.DidNotReceive().AddAsync(Arg.Any<Lancamento>(), Arg.Any<CancellationToken>());
        await _unitOfWork.DidNotReceive().SaveChangesAsync(Arg.Any<CancellationToken>());
    }
}
