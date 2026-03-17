using CashFlow.Shared.Domain.Abstractions;

namespace CashFlow.Consolidado.Domain.Errors;

public static class ConsolidadoErrors
{
    public static Error NaoEncontrado(DateOnly data) =>
        Error.Validation(
            "Consolidado.NaoEncontrado",
            $"Consolidado para a data '{data:dd/MM/yyyy}' não foi encontrado.");

    public static readonly Error PeriodoInvalido =
        Error.Validation(
            "Consolidado.PeriodoInvalido",
            "A data de início deve ser anterior ou igual a data de fim.");
}
