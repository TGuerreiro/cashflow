namespace CashFlow.Shared.Domain.Abstractions;

public class Result
{
    protected Result(bool isSuccess, Error error)
    {
        if (isSuccess && error != Error.None)
            throw new InvalidOperationException("Um resultado de sucesso não pode conter um erro.");

        if (!isSuccess && error == Error.None)
            throw new InvalidOperationException("Um resultado de falha deve conter um erro.");

        IsSuccess = isSuccess;
        Error = error;
    }

    public bool IsSuccess { get; }
    public bool IsFailure => !IsSuccess;
    public Error Error { get; }

    public static Result Success() => new(true, Error.None);
    public static Result Failure(Error error) => new(false, error);

    public static Result<TValue> Success<TValue>(TValue value) =>
        Result<TValue>.Success(value);

    public static Result<TValue> Failure<TValue>(Error error) =>
        Result<TValue>.Failure(error);
}

public class Result<TValue> : Result
{
    private readonly TValue? _value;

    private Result(TValue? value, bool isSuccess, Error error)
        : base(isSuccess, error)
    {
        _value = value;
    }

    public TValue Value =>
        IsSuccess
            ? _value!
            : throw new InvalidOperationException("Não é possível acessar o valor de um resultado de falha.");

    public static Result<TValue> Success(TValue value) =>
        new(value, true, Error.None);

    public new static Result<TValue> Failure(Error error) =>
        new(default, false, error);

    public Result<TOut> Map<TOut>(Func<TValue, TOut> mapper) =>
        IsSuccess
            ? Result<TOut>.Success(mapper(Value))
            : Result<TOut>.Failure(Error);
}
