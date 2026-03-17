using CashFlow.Lancamentos.Domain.Events;
using CashFlow.Shared.Domain.Abstractions;
using CashFlow.Shared.Domain.Interfaces;
using CashFlow.Shared.Messaging.Events;
using MassTransit;
using Microsoft.Extensions.Logging;

namespace CashFlow.Lancamentos.Infrastructure.Persistence;

public sealed class UnitOfWork(
    LancamentosDbContext context,
    IPublishEndpoint publishEndpoint,
    ILogger<UnitOfWork> logger) : IUnitOfWork
{
    private readonly LancamentosDbContext _context = context;
    private readonly IPublishEndpoint _publishEndpoint = publishEndpoint;
    private readonly ILogger<UnitOfWork> _logger = logger;

    public async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        var domainEvents = _context.ChangeTracker
            .Entries<Entity>()
            .Where(e => e.Entity.DomainEvents.Count > 0)
            .SelectMany(e => e.Entity.DomainEvents)
            .ToList();

        _context.ChangeTracker
            .Entries<Entity>()
            .Where(e => e.Entity.DomainEvents.Count > 0)
            .ToList()
            .ForEach(e => e.Entity.ClearDomainEvents());

        await using var transaction = await _context.Database.BeginTransactionAsync(cancellationToken);

        try
        {
            foreach (var domainEvent in domainEvents)
            {
                var integrationEvent = MapToIntegrationEvent(domainEvent);
                if (integrationEvent is not null)
                {
                    await _publishEndpoint.Publish(
                        integrationEvent,
                        integrationEvent.GetType(),
                        cancellationToken);

                    _logger.LogInformation(
                        "Outbox: evento enfileirado {EventType}",
                        integrationEvent.GetType().Name);
                }
            }

            var result = await _context.SaveChangesAsync(cancellationToken);

            await transaction.CommitAsync(cancellationToken);

            return result;
        }
        catch
        {
            await transaction.RollbackAsync(cancellationToken);
            throw;
        }
    }

    private static object? MapToIntegrationEvent(IDomainEvent domainEvent)
    {
        return domainEvent switch
        {
            LancamentoRegistradoDomainEvent e => new LancamentoCriadoEvent
            {
                LancamentoId = e.LancamentoId,
                Data = e.Data,
                Valor = e.Valor,
                Tipo = e.Tipo,
                Descricao = e.Descricao
            },
            LancamentoAtualizadoDomainEvent e => new LancamentoAtualizadoEvent
            {
                LancamentoId = e.LancamentoId,
                Data = e.Data,
                ValorAnterior = e.ValorAnterior,
                ValorNovo = e.ValorNovo,
                TipoAnterior = e.TipoAnterior,
                TipoNovo = e.TipoNovo
            },
            LancamentoRemovidoDomainEvent e => new LancamentoRemovidoEvent
            {
                LancamentoId = e.LancamentoId,
                Data = e.Data,
                Valor = e.Valor,
                Tipo = e.Tipo
            },
            _ => null
        };
    }
}