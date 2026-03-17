using CashFlow.Consolidado.Domain.Entities;
using CashFlow.Shared.Domain.Enums;
using FluentAssertions;

namespace CashFlow.Consolidado.UnitTests;

public class ConsolidadoDiarioTests
{
    [Fact]
    public void Criar_DeveInicializarComValoresZerados()
    {
        // Arrange
        var data = DateOnly.FromDateTime(DateTime.Now);

        // Act
        var consolidado = ConsolidadoDiario.Criar(data);

        // Assert
        consolidado.Data.Should().Be(data);
        consolidado.TotalCreditos.Should().Be(0);
        consolidado.TotalDebitos.Should().Be(0);
        consolidado.Saldo.Should().Be(0);
        consolidado.QuantidadeLancamentos.Should().Be(0);
    }

    [Fact]
    public void AplicarLancamento_Credito_DeveAumentarSaldoETotalCreditos()
    {
        // Arrange
        var consolidado = ConsolidadoDiario.Criar(DateOnly.FromDateTime(DateTime.Now));
        var valor = 100.50m;

        // Act
        consolidado.AplicarLancamento(valor, TipoLancamento.Credito);

        // Assert
        consolidado.TotalCreditos.Should().Be(valor);
        consolidado.Saldo.Should().Be(valor);
        consolidado.QuantidadeLancamentos.Should().Be(1);
    }

    [Fact]
    public void AplicarLancamento_Debito_DeveDiminuirSaldoEAumentarTotalDebitos()
    {
        // Arrange
        var consolidado = ConsolidadoDiario.Criar(DateOnly.FromDateTime(DateTime.Now));
        var valor = 50.25m;

        // Act
        consolidado.AplicarLancamento(valor, TipoLancamento.Debito);

        // Assert
        consolidado.TotalDebitos.Should().Be(valor);
        consolidado.Saldo.Should().Be(-valor);
        consolidado.QuantidadeLancamentos.Should().Be(1);
    }

    [Fact]
    public void ReverterLancamento_Credito_DeveDiminuirSaldoETotalCreditos()
    {
        // Arrange
        var consolidado = ConsolidadoDiario.Criar(DateOnly.FromDateTime(DateTime.Now));
        consolidado.AplicarLancamento(100m, TipoLancamento.Credito);

        // Act
        consolidado.ReverterLancamento(100m, TipoLancamento.Credito);

        // Assert
        consolidado.TotalCreditos.Should().Be(0);
        consolidado.Saldo.Should().Be(0);
        consolidado.QuantidadeLancamentos.Should().Be(0);
    }
}
