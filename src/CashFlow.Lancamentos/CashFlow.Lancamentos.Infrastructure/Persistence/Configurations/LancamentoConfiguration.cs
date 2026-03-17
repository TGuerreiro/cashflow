using CashFlow.Lancamentos.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CashFlow.Lancamentos.Infrastructure.Persistence.Configurations;

public class LancamentoConfiguration : IEntityTypeConfiguration<Lancamento>
{
    public void Configure(EntityTypeBuilder<Lancamento> builder)
    {
        builder.ToTable("lancamentos");

        builder.HasKey(l => l.Id);

        builder.Property(l => l.Id)
            .HasColumnName("id")
            .ValueGeneratedNever();

        builder.Property(l => l.Data)
            .HasColumnName("data")
            .IsRequired();

        builder.Property(l => l.Tipo)
            .HasColumnName("tipo")
            .IsRequired()
            .HasConversion<int>();

        builder.Property(l => l.Descricao)
            .HasColumnName("descricao")
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(l => l.CreatedAt)
            .HasColumnName("created_at")
            .IsRequired();

        builder.Property(l => l.UpdatedAt)
            .HasColumnName("updated_at");

        builder.OwnsOne(l => l.Valor, money =>
        {
            money.Property(m => m.Amount)
                .HasColumnName("valor")
                .HasColumnType("decimal(18,2)")
                .IsRequired();
        });

        builder.HasIndex(l => l.Data)
            .HasDatabaseName("ix_lancamentos_data");

        // Ignora domain events (não persiste)
        builder.Ignore(l => l.DomainEvents);
    }
}