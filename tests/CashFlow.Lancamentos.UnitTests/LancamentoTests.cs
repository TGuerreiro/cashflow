using CashFlow.Lancamentos.Domain.Entities;
using CashFlow.Lancamentos.Domain.Events;
using CashFlow.Shared.Domain.Enums;
using FluentAssertions;

namespace CashFlow.Lancamentos.UnitTests;

public class LancamentoTests
{
    [Fact]
    public void Criar_ComDadosValidos_DeveRetornarSucesso()
    {
        // Arrange
        var data = DateOnly.FromDateTime(DateTime.Now);
        var valor = 100.50m;
        var tipo = TipoLancamento.Credito;
        var descricao = "Venda de Produto";

        // Act
        var result = Lancamento.Criar(data, valor, tipo, descricao);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Value.Should().NotBeNull();
        result.Value.Data.Should().Be(data);
        result.Value.Valor.Amount.Should().Be(valor);
        result.Value.Tipo.Should().Be(tipo);
        result.Value.Descricao.Should().Be(descricao);
        result.Value.DomainEvents.Should().ContainSingle(e => e is LancamentoRegistradoDomainEvent);
    }

    [Theory]
    [InlineData("")]
    [InlineData(" ")]
    public void Criar_ComDescricaoInvalida_DeveRetornarFalha(string descricao)
    {
        // Arrange
        var data = DateOnly.FromDateTime(DateTime.Now);
        var valor = 100.50m;
        var tipo = TipoLancamento.Credito;

        // Act
        var result = Lancamento.Criar(data, valor, tipo, descricao!);

        // Assert
        result.IsFailure.Should().BeTrue();
        result.Error.Code.Should().Be("Lancamento.DescricaoVazia");
    }

    [Fact]
    public void Criar_ComValorNegativo_DeveRetornarFalha()
    {
        // Arrange
        var data = DateOnly.FromDateTime(DateTime.Now);
        var valor = -10.00m;
        var tipo = TipoLancamento.Credito;
        var descricao = "Teste";

        // Act
        var result = Lancamento.Criar(data, valor, tipo, descricao);

        // Assert
        result.IsFailure.Should().BeTrue();
        result.Error.Code.Should().Be("Money.InvalidAmount");
    }

    [Fact]
    public void Atualizar_ComDadosValidos_DeveAlterarEstadoERaiseEvent()
    {
        // Arrange
        var lancamento = Lancamento.Criar(DateOnly.FromDateTime(DateTime.Now), 100m, TipoLancamento.Credito, "Original").Value;
        var novaData = DateOnly.FromDateTime(DateTime.Now.AddDays(1));
        var novoValor = 200m;
        var novoTipo = TipoLancamento.Debito;
        var novaDescricao = "Atualizado";

        // Act
        var result = lancamento.Atualizar(novaData, novoValor, novoTipo, novaDescricao);

        // Assert
        result.IsSuccess.Should().BeTrue();
        lancamento.Data.Should().Be(novaData);
        lancamento.Valor.Amount.Should().Be(novoValor);
        lancamento.Tipo.Should().Be(novoTipo);
        lancamento.Descricao.Should().Be(novaDescricao);
        lancamento.DomainEvents.Should().ContainSingle(e => e is LancamentoAtualizadoDomainEvent);
    }

    [Fact]
    public void MarcarParaRemocao_DeveLevantarEventoDeRemocao()
    {
        // Arrange
        var lancamento = Lancamento.Criar(DateOnly.FromDateTime(DateTime.Now), 100m, TipoLancamento.Credito, "Teste").Value;

        // Act
        lancamento.MarcarParaRemocao();

        // Assert
        lancamento.DomainEvents.Should().ContainSingle(e => e is LancamentoRemovidoDomainEvent);
    }
}
