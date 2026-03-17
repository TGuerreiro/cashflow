using MassTransit;

namespace CashFlow.Consolidado.Infrastructure.Messaging.Consumers;

public class LancamentoCriadoConsumerDefinition : ConsumerDefinition<LancamentoCriadoConsumer>
{
    protected override void ConfigureConsumer(
        IReceiveEndpointConfigurator endpointConfigurator,
        IConsumerConfigurator<LancamentoCriadoConsumer> consumerConfigurator,
        IRegistrationContext context)
    {
        // Retry com backoff exponencial
        endpointConfigurator.UseMessageRetry(r => r
            .Exponential(5, TimeSpan.FromSeconds(1), TimeSpan.FromSeconds(30), TimeSpan.FromSeconds(2)));

        // Circuit breaker
        endpointConfigurator.UseCircuitBreaker(cb =>
        {
            cb.TrackingPeriod = TimeSpan.FromMinutes(1);
            cb.TripThreshold = 15;
            cb.ActiveThreshold = 10;
            cb.ResetInterval = TimeSpan.FromMinutes(5);
        });

        // Prefetch para performance
        endpointConfigurator.PrefetchCount = 16;
    }
}
