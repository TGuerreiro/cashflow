using CashFlow.Shared.Domain.Abstractions;

namespace CashFlow.Lancamentos.Domain.Errors;

public static class LancamentoErrors
{
    public static Error NaoEncontrado(Guid id) =>
        Error.NotFound("Lancamento", id);

    public static readonly Error DescricaoVazia =
        Error.Validation("Lancamento.DescricaoVazia", "A descrição não pode ser vazia.");

    public static readonly Error DescricaoMuitoLonga =
        Error.Validation("Lancamento.DescricaoMuitoLonga", "A descrição não pode ter mais que 200 caracteres.");

    public static readonly Error ValorInvalido =
        Error.Validation("Lancamento.ValorInvalido", "O valor deve ser maior que zero.");

    public static readonly Error TipoInvalido =
        Error.Validation("Lancamento.TipoInvalido", "O tipo de lançamento é inválido.");
}
