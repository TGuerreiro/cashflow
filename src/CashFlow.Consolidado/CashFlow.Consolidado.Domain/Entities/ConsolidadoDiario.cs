using CashFlow.Shared.Domain.Abstractions;
using CashFlow.Shared.Domain.Enums;

namespace CashFlow.Consolidado.Domain.Entities;

public class ConsolidadoDiario : AggregateRoot
{
    private ConsolidadoDiario() { } // Necessário para o EF

    private ConsolidadoDiario(Guid id, DateOnly data)
    {
        Id = id;
        Data = data;
        TotalCreditos = 0;
        TotalDebitos = 0;
        Saldo = 0;
        QuantidadeLancamentos = 0;
        CreatedAt = DateTime.UtcNow;
    }

    public DateOnly Data { get; private set; }
    public decimal TotalCreditos { get; private set; }
    public decimal TotalDebitos { get; private set; }
    public decimal Saldo { get; private set; }
    public int QuantidadeLancamentos { get; private set; }

    public static ConsolidadoDiario Criar(DateOnly data)
    {
        return new ConsolidadoDiario(Guid.NewGuid(), data);
    }

    public void AplicarLancamento(decimal valor, TipoLancamento tipo)
    {
        switch (tipo)
        {
            case TipoLancamento.Credito:
                TotalCreditos += valor;
                Saldo += valor;
                break;
            case TipoLancamento.Debito:
                TotalDebitos += valor;
                Saldo -= valor;
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(tipo));
        }

        QuantidadeLancamentos++;

        UpdatedAt = DateTime.UtcNow;
    }

    public void ReverterLancamento(decimal valor, TipoLancamento tipo)
    {
        switch (tipo)
        {
            case TipoLancamento.Credito:
                TotalCreditos -= valor;
                Saldo -= valor;
                break;
            case TipoLancamento.Debito:
                TotalDebitos -= valor;
                Saldo += valor;
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(tipo));
        }

        QuantidadeLancamentos--;
        UpdatedAt = DateTime.UtcNow;
    }
}
