using CashFlow.Consolidado.Application.Services;
using CashFlow.Consolidado.Domain.Repositories;
using CashFlow.Consolidado.Infrastructure.Messaging.Consumers;
using CashFlow.Consolidado.Infrastructure.Persistence;
using CashFlow.Consolidado.Infrastructure.Repositories;
using CashFlow.Shared.Domain.Interfaces;
using CashFlow.Shared.Messaging.Events;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace CashFlow.Consolidado.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddConsolidadoInfrastructure(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        // EF Core + PostgreSQL
        services.AddDbContext<ConsolidadoDbContext>(options =>
            options.UseNpgsql(
                configuration.GetConnectionString("ConsolidadoDb"),
                npgsql => npgsql.MigrationsAssembly(typeof(ConsolidadoDbContext).Assembly.FullName)));

        // Repositories + Services
        services.AddScoped<IConsolidadoDiarioRepository, ConsolidadoDiarioRepository>();
        services.AddScoped<IConsolidacaoService, ConsolidacaoService>();
        services.AddScoped<IUnitOfWork, CashFlow.Consolidado.Infrastructure.Persistence.UnitOfWork>();

        // MassTransit + RabbitMQ
        services.AddMassTransit(bus =>
        {
            // Consumers
            bus.AddConsumer<LancamentoCriadoConsumer>();
            bus.AddConsumer<LancamentoAtualizadoConsumer>();
            bus.AddConsumer<LancamentoRemovidoConsumer>();

            // Configura o Outbox/Inbox do EF para idempotência
            bus.AddEntityFrameworkOutbox<ConsolidadoDbContext>(o =>
            {
                o.UsePostgres();
                o.UseBusOutbox();
            });

            bus.UsingRabbitMq((context, cfg) =>
            {
                cfg.Host(configuration.GetConnectionString("RabbitMq") ?? "rabbitmq", h =>
                {
                    h.Username(configuration["RabbitMq:Username"] ?? "guest");
                    h.Password(configuration["RabbitMq:Password"] ?? "guest");
                });

                // Resiliência Global (Polly)
                cfg.UseMessageRetry(r => r.Incremental(3, TimeSpan.FromSeconds(1), TimeSpan.FromSeconds(2)));

                cfg.ReceiveEndpoint("lancamento-criado-event", e =>
                {
                    e.Bind<LancamentoCriadoEvent>();
                    e.UseEntityFrameworkOutbox<ConsolidadoDbContext>(context);
                    e.ConfigureConsumer<LancamentoCriadoConsumer>(context);
                });

                cfg.ReceiveEndpoint("lancamento-atualizado-event", e =>
                {
                    e.Bind<LancamentoAtualizadoEvent>();
                    e.UseEntityFrameworkOutbox<ConsolidadoDbContext>(context);
                    e.ConfigureConsumer<LancamentoAtualizadoConsumer>(context);
                });

                cfg.ReceiveEndpoint("lancamento-removido-event", e =>
                {
                    e.Bind<LancamentoRemovidoEvent>();
                    e.UseEntityFrameworkOutbox<ConsolidadoDbContext>(context);
                    e.ConfigureConsumer<LancamentoRemovidoConsumer>(context);
                });
            });
        });

        return services;
    }
}
