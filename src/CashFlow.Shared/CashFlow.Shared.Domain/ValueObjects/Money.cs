using CashFlow.Shared.Domain.Abstractions;

namespace CashFlow.Shared.Domain.ValueObjects;

public  class Money : ValueObject
{
    private Money(decimal amount)
    {
        Amount = amount;
    }

    public decimal Amount { get; }

    public static Result<Money> Create(decimal amount)
    {
        if (amount <= 0)
            return Result<Money>.Failure(
                Error.Validation("Money.InvalidAmount", "O valor deve ser maior que zero."));

        return Result<Money>.Success(new Money(Math.Round(amount, 2)));
    }

    public static Money Zero() => new(0);

    public Money Add(decimal amount) => new(Amount + amount);
    public Money Subtract(decimal amount) => new(Amount - amount);

    public Money Add(Money other) => new(Amount + other.Amount);
    public Money Subtract(Money other) => new(Amount - other.Amount);

    protected override IEnumerable<object?> GetEqualityComponents()
    {
        yield return Amount;
    }

    public override string ToString() => $"R$ {Amount:N2}";

    public static implicit operator decimal(Money money) => money.Amount;
}
