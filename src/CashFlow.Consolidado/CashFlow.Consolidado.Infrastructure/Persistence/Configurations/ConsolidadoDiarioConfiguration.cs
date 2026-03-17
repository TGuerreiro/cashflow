using CashFlow.Consolidado.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CashFlow.Consolidado.Infrastructure.Persistence.Configurations;

public class ConsolidadoDiarioConfiguration : IEntityTypeConfiguration<ConsolidadoDiario>
{
    public void Configure(EntityTypeBuilder<ConsolidadoDiario> builder)
    {
        builder.ToTable("consolidados_diarios");

        builder.HasKey(c => c.Id);

        builder.Property(c => c.Id)
            .HasColumnName("id")
            .ValueGeneratedNever();

        builder.Property(c => c.Data)
            .HasColumnName("data")
            .IsRequired();

        builder.Property(c => c.TotalCreditos)
            .HasColumnName("total_creditos")
            .HasColumnType("decimal(18,2)")
            .IsRequired();

        builder.Property(c => c.TotalDebitos)
            .HasColumnName("total_debitos")
            .HasColumnType("decimal(18,2)")
            .IsRequired();

        builder.Property(c => c.Saldo)
            .HasColumnName("saldo")
            .HasColumnType("decimal(18,2)")
            .IsRequired();

        builder.Property(c => c.QuantidadeLancamentos)
            .HasColumnName("quantidade_lancamentos")
            .IsRequired();

        builder.Property(c => c.CreatedAt)
            .HasColumnName("created_at")
            .IsRequired();

        builder.Property(c => c.UpdatedAt)
            .HasColumnName("updated_at");

        builder.HasIndex(c => c.Data)
            .IsUnique()
            .HasDatabaseName("ix_consolidados_diarios_data");

        builder.Ignore(c => c.DomainEvents);
    }
}
