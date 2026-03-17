using CashFlow.Consolidado.Domain.Entities;
using CashFlow.Consolidado.Domain.Repositories;
using CashFlow.Shared.Domain.Enums;
using CashFlow.Shared.Domain.Interfaces;
using Microsoft.Extensions.Logging;

namespace CashFlow.Consolidado.Application.Services;

public class ConsolidacaoService(
    IConsolidadoDiarioRepository repository,
    IUnitOfWork unitOfWork,
    ILogger<ConsolidacaoService> logger) : IConsolidacaoService
{
    private readonly IConsolidadoDiarioRepository _repository = repository;
    private readonly IUnitOfWork _unitOfWork = unitOfWork;
    private readonly ILogger<ConsolidacaoService> _logger = logger;

    public async Task AplicarLancamentoAsync(
        DateOnly data,
        decimal valor,
        TipoLancamento tipo,
        CancellationToken cancellationToken = default)
    {
        var consolidado = await ObterOuCriarConsolidadoAsync(data, cancellationToken);

        consolidado.AplicarLancamento(valor, tipo);

        _logger.LogInformation(
            "Lançamento aplicado no consolidado {Data}: {Tipo} R$ {Valor:N2} | Saldo: R$ {Saldo:N2}",
            data, tipo, valor, consolidado.Saldo);

        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }

    public async Task ReverterLancamentoAsync(
        DateOnly data,
        decimal valor,
        TipoLancamento tipo,
        CancellationToken cancellationToken = default)
    {
        var consolidado = await _repository.GetByDataAsync(data, cancellationToken);

        if (consolidado is null)
        {
            _logger.LogWarning(
                "Tentativa de reverter o lançamento no consolidado inexistente para data {Data}", data);
            return;
        }

        consolidado.ReverterLancamento(valor, tipo);

        _logger.LogInformation(
            "Lançamento revertido no consolidado {Data}: {Tipo} R$ {Valor:N2} | Saldo: R$ {Saldo:N2}",
            data, tipo, valor, consolidado.Saldo);

        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }

    private async Task<ConsolidadoDiario> ObterOuCriarConsolidadoAsync(
        DateOnly data,
        CancellationToken cancellationToken)
    {
        var consolidado = await _repository.GetByDataAsync(data, cancellationToken);

        if (consolidado is not null)
            return consolidado;

        consolidado = ConsolidadoDiario.Criar(data);
       
        await _repository.AddAsync(consolidado, cancellationToken);

        _logger.LogInformation("Consolidado criado para data {Data}", data);

        return consolidado;
    }
}
