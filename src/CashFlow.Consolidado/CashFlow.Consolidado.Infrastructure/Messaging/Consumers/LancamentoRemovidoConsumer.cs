using CashFlow.Consolidado.Application.Services;
using CashFlow.Shared.Messaging.Events;
using MassTransit;
using Microsoft.Extensions.Logging;

namespace CashFlow.Consolidado.Infrastructure.Messaging.Consumers;

public class LancamentoRemovidoConsumer(
    IConsolidacaoService consolidacaoService,
    ILogger<LancamentoRemovidoConsumer> logger) : IConsumer<LancamentoRemovidoEvent>
{
    private readonly IConsolidacaoService _consolidacaoService = consolidacaoService;
    private readonly ILogger<LancamentoRemovidoConsumer> _logger = logger;

    public async Task Consume(ConsumeContext<LancamentoRemovidoEvent> context)
    {
        var message = context.Message;

        _logger.LogInformation(
            "Evento Recebido - LancamentoRemovidoEvent: LancamentoId={LancamentoId}, Data={Data}",
            message.LancamentoId, message.Data);

        await _consolidacaoService.ReverterLancamentoAsync(
            message.Data,
            message.Valor,
            message.Tipo,
            context.CancellationToken);
    }
}
