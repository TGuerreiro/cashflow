using CashFlow.Consolidado.Application.Services;
using CashFlow.Shared.Messaging.Events;
using MassTransit;
using Microsoft.Extensions.Logging;

namespace CashFlow.Consolidado.Infrastructure.Messaging.Consumers;

public class LancamentoCriadoConsumer(
    IConsolidacaoService consolidacaoService,
    ILogger<LancamentoCriadoConsumer> logger) : IConsumer<LancamentoCriadoEvent>
{
    private readonly IConsolidacaoService _consolidacaoService = consolidacaoService;
    private readonly ILogger<LancamentoCriadoConsumer> _logger = logger;

    public async Task Consume(ConsumeContext<LancamentoCriadoEvent> context)
    {
        var message = context.Message;

        _logger.LogInformation(
            "Evento Recebido - Lancamento Criado - Event: LancamentoId={LancamentoId}, Data={Data}, Valor={Valor}, Tipo={Tipo}",
            message.LancamentoId, message.Data, message.Valor, message.Tipo);

        await _consolidacaoService.AplicarLancamentoAsync(
            message.Data,
            message.Valor,
            message.Tipo,
            context.CancellationToken);
    }
}
