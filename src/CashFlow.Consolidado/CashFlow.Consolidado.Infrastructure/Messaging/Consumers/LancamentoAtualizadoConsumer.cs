using CashFlow.Consolidado.Application.Services;
using CashFlow.Shared.Messaging.Events;
using MassTransit;
using Microsoft.Extensions.Logging;

namespace CashFlow.Consolidado.Infrastructure.Messaging.Consumers;

public class LancamentoAtualizadoConsumer(
    IConsolidacaoService consolidacaoService,
    ILogger<LancamentoAtualizadoConsumer> logger) : IConsumer<LancamentoAtualizadoEvent>
{
    private readonly IConsolidacaoService _consolidacaoService = consolidacaoService;
    private readonly ILogger<LancamentoAtualizadoConsumer> _logger = logger;

    public async Task Consume(ConsumeContext<LancamentoAtualizadoEvent> context)
    {
        var message = context.Message;

        _logger.LogInformation(
            "Lancamento recebido atualizado - Event: LancamentoId={LancamentoId}",
            message.LancamentoId);

        // Reverte o lancamento anterior
        await _consolidacaoService.ReverterLancamentoAsync(
            message.Data,
            message.ValorAnterior,
            message.TipoAnterior,
            context.CancellationToken);

        // Aplica o novo lancamento
        await _consolidacaoService.AplicarLancamentoAsync(
            message.Data,
            message.ValorNovo,
            message.TipoNovo,
            context.CancellationToken);
    }
}
