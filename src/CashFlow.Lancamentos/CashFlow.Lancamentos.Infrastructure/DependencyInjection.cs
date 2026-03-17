using CashFlow.Lancamentos.Domain.Repositories;
using CashFlow.Lancamentos.Infrastructure.Persistence;
using CashFlow.Lancamentos.Infrastructure.Repositories;
using CashFlow.Shared.Domain.Interfaces;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace CashFlow.Lancamentos.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddLancamentosInfrastructure(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        // EF Core + PostgreSQL
        services.AddDbContext<LancamentosDbContext>(options =>
            options.UseNpgsql(
                configuration.GetConnectionString("LancamentosDb"),
                npgsql => npgsql.MigrationsAssembly(typeof(LancamentosDbContext).Assembly.FullName)));

        // Repositories
        services.AddScoped<ILancamentoRepository, LancamentoRepository>();
        services.AddScoped<IUnitOfWork, UnitOfWork>();

        // MassTransit + RabbitMQ 
        services.AddMassTransit(bus =>
        {
            bus.SetKebabCaseEndpointNameFormatter();

            bus.AddEntityFrameworkOutbox<LancamentosDbContext>(outbox =>
            {
                outbox.UsePostgres();
                outbox.UseBusOutbox();
                outbox.QueryDelay = TimeSpan.FromSeconds(5);
            });

            bus.UsingRabbitMq((context, cfg) =>
            {
                cfg.Host(configuration.GetConnectionString("RabbitMq") ?? "rabbitmq://localhost", h =>
                {
                    h.Username(configuration["RabbitMq:Username"] ?? "guest");
                    h.Password(configuration["RabbitMq:Password"] ?? "guest");
                });

                cfg.ConfigureEndpoints(context);
            });
        });

        return services;
    }
}
