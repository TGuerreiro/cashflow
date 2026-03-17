using CashFlow.Consolidado.Application.Behaviors;
using CashFlow.Consolidado.Application.Services;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;

namespace CashFlow.Consolidado.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddConsolidadoApplication(this IServiceCollection services)
    {
        var assembly = typeof(DependencyInjection).Assembly;

        services.AddMediatR(config =>
        {
            config.RegisterServicesFromAssembly(assembly);
            config.AddOpenBehavior(typeof(LoggingBehavior<,>));
        });

        services.AddValidatorsFromAssembly(assembly);
        services.AddScoped<IConsolidacaoService, ConsolidacaoService>();

        return services;
    }
}