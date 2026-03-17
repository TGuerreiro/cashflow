using CashFlow.Consolidado.Domain.Entities;
using MassTransit;
using Microsoft.EntityFrameworkCore;

namespace CashFlow.Consolidado.Infrastructure.Persistence;

public class ConsolidadoDbContext(DbContextOptions<ConsolidadoDbContext> options) : DbContext(options)
{
    public DbSet<ConsolidadoDiario> Consolidados => Set<ConsolidadoDiario>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(ConsolidadoDbContext).Assembly);

        modelBuilder.AddInboxStateEntity();
        modelBuilder.AddOutboxMessageEntity();
        modelBuilder.AddOutboxStateEntity();
    }
}
