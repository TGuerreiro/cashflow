using CashFlow.Lancamentos.Domain.Events;
using CashFlow.Shared.Domain.Abstractions;
using CashFlow.Shared.Domain.Enums;
using CashFlow.Shared.Domain.ValueObjects;

namespace CashFlow.Lancamentos.Domain.Entities;

public class Lancamento : AggregateRoot
{
    private Lancamento() { } // Necessário para o EF

    private Lancamento(
        Guid id,
        DateOnly data,
        Money valor,
        TipoLancamento tipo,
        string descricao)
    {
        Id = id;
        Data = data;
        Valor = valor;
        Tipo = tipo;
        Descricao = descricao;
        CreatedAt = DateTime.UtcNow;
    }

    public DateOnly Data { get; private set; }
    public Money Valor { get; private set; } = null!;
    public TipoLancamento Tipo { get; private set; }
    public string Descricao { get; private set; } = string.Empty;

    public static Result<Lancamento> Criar(
        DateOnly data,
        decimal valor,
        TipoLancamento tipo,
        string descricao)
    {
        var moneyResult = Money.Create(valor);
        if (moneyResult.IsFailure)
            return Result<Lancamento>.Failure(moneyResult.Error);

        if (string.IsNullOrWhiteSpace(descricao))
            return Result<Lancamento>.Failure(
                Error.Validation("Lancamento.DescricaoVazia", "A descrição não pode ser vazia."));

        if (descricao.Length > 200)
            return Result<Lancamento>.Failure(
                Error.Validation("Lancamento.DescricaoMuitoLonga", "A descrição não pode ter mais que 200 caracteres."));

        if (!Enum.IsDefined(tipo))
            return Result<Lancamento>.Failure(
                Error.Validation("Lancamento.TipoInvalido", "O tipo de lançamento é inválido."));

        var lancamento = new Lancamento(
            Guid.NewGuid(),
            data,
            moneyResult.Value,
            tipo,
            descricao.Trim());

        lancamento.RaiseDomainEvent(new LancamentoRegistradoDomainEvent(
            lancamento.Id,
            lancamento.Data,
            lancamento.Valor.Amount,
            lancamento.Tipo,
            lancamento.Descricao));

        return Result<Lancamento>.Success(lancamento);
    }

    public Result Atualizar(
        DateOnly data,
        decimal novoValor,
        TipoLancamento novoTipo,
        string novaDescricao)
    {
        var moneyResult = Money.Create(novoValor);
        if (moneyResult.IsFailure)
            return Result.Failure(moneyResult.Error);

        if (string.IsNullOrWhiteSpace(novaDescricao))
            return Result.Failure(
                Error.Validation("Lancamento.DescricaoVazia", "A descrição não pode ser vazia."));

        if (novaDescricao.Length > 200)
            return Result.Failure(
                Error.Validation("Lancamento.DescricaoMuitoLonga", "A descrição não pode ter mais que 200 caracteres."));

        if (!Enum.IsDefined(novoTipo))
            return Result.Failure(
                Error.Validation("Lancamento.TipoInvalido", "O tipo de lançamento é inválido."));

        var valorAnterior = Valor.Amount;
        var tipoAnterior = Tipo;

        Data = data;
        Valor = moneyResult.Value;
        Tipo = novoTipo;
        Descricao = novaDescricao.Trim();
        UpdatedAt = DateTime.UtcNow;

        RaiseDomainEvent(new LancamentoAtualizadoDomainEvent(
            Id,
            Data,
            valorAnterior,
            Valor.Amount,
            tipoAnterior,
            Tipo));

        return Result.Success();
    }

    public void MarcarParaRemocao()
    {
        RaiseDomainEvent(new LancamentoRemovidoDomainEvent(
            Id,
            Data,
            Valor.Amount,
            Tipo));
    }
}
