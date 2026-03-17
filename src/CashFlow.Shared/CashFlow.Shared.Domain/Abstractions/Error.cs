namespace CashFlow.Shared.Domain.Abstractions;

public record Error(string Code, string Message)
{
    public static readonly Error None = new(string.Empty, string.Empty);
    public static readonly Error NullValue = new("Error.NullValue", "O valor fornecido é nulo.");

    public static Error NotFound(string entity, Guid id) =>
        new($"{entity}.NotFound", $"{entity} com Id '{id}' não foi encontrado.");

    public static Error Validation(string code, string message) =>
        new(code, message);

    public static Error Conflict(string code, string message) =>
        new(code, message);
}
