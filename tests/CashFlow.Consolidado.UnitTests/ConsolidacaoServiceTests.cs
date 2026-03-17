using CashFlow.Consolidado.Application.Services;
using CashFlow.Consolidado.Domain.Entities;
using CashFlow.Consolidado.Domain.Repositories;
using CashFlow.Shared.Domain.Enums;
using CashFlow.Shared.Domain.Interfaces;
using Microsoft.Extensions.Logging;
using NSubstitute;

namespace CashFlow.Consolidado.UnitTests;

public class ConsolidacaoServiceTests
{
    private readonly IConsolidadoDiarioRepository _repository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<ConsolidacaoService> _logger;
    private readonly ConsolidacaoService _service;

    public ConsolidacaoServiceTests()
    {
        _repository = Substitute.For<IConsolidadoDiarioRepository>();
        _unitOfWork = Substitute.For<IUnitOfWork>();
        _logger = Substitute.For<ILogger<ConsolidacaoService>>();
        _service = new ConsolidacaoService(_repository, _unitOfWork, _logger);
    }

    [Fact]
    public async Task AplicarLancamentoAsync_QuandoConsolidadoNaoExiste_DeveCriarEAplicar()
    {
        // Arrange
        var data = DateOnly.FromDateTime(DateTime.Now);
        var valor = 100m;
        var tipo = TipoLancamento.Credito;
        _repository.GetByDataAsync(data, Arg.Any<CancellationToken>()).Returns((ConsolidadoDiario)null!);

        // Act
        await _service.AplicarLancamentoAsync(data, valor, tipo, CancellationToken.None);

        // Assert
        await _repository.Received(1).AddAsync(Arg.Any<ConsolidadoDiario>(), Arg.Any<CancellationToken>());
        await _unitOfWork.Received(1).SaveChangesAsync(Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task AplicarLancamentoAsync_QuandoConsolidadoExiste_DeveApenasAplicar()
    {
        // Arrange
        var data = DateOnly.FromDateTime(DateTime.Now);
        var consolidado = ConsolidadoDiario.Criar(data);
        _repository.GetByDataAsync(data, Arg.Any<CancellationToken>()).Returns(consolidado);

        // Act
        await _service.AplicarLancamentoAsync(data, 100m, TipoLancamento.Credito, CancellationToken.None);

        // Assert
        await _repository.DidNotReceive().AddAsync(Arg.Any<ConsolidadoDiario>(), Arg.Any<CancellationToken>());
        await _unitOfWork.Received(1).SaveChangesAsync(Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task ReverterLancamentoAsync_QuandoConsolidadoNaoExiste_DeveLogarAviso()
    {
        // Arrange
        var data = DateOnly.FromDateTime(DateTime.Now);
        _repository.GetByDataAsync(data, Arg.Any<CancellationToken>()).Returns((ConsolidadoDiario)null!);

        // Act
        await _service.ReverterLancamentoAsync(data, 100m, TipoLancamento.Credito, CancellationToken.None);

        // Assert
        await _unitOfWork.DidNotReceive().SaveChangesAsync(Arg.Any<CancellationToken>());
    }
}
