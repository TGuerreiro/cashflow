using CashFlow.Shared.Domain.Abstractions;
using FluentValidation;
using MediatR;

namespace CashFlow.Lancamentos.Application.Behaviors;

public class ValidationBehavior<TRequest, TResponse>(IEnumerable<IValidator<TRequest>> validators)
    : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IRequest<TResponse>
    where TResponse : Result
{
    private readonly IEnumerable<IValidator<TRequest>> _validators = validators;

    public async Task<TResponse> Handle(
        TRequest request,
        RequestHandlerDelegate<TResponse> next,
        CancellationToken cancellationToken)
    {
        if (!_validators.Any())
            return await next();

        var context = new ValidationContext<TRequest>(request);

        var validationResults = await Task.WhenAll(
            _validators.Select(v => v.ValidateAsync(context, cancellationToken)));

        var firstFailure = validationResults
            .SelectMany(r => r.Errors)
            .FirstOrDefault(f => f is not null);

        if (firstFailure is null)
            return await next();

        var error = Error.Validation(firstFailure.PropertyName, firstFailure.ErrorMessage);

        if (typeof(TResponse).IsGenericType)
        {
            var failureMethod = typeof(TResponse)
                .GetMethod(nameof(Result.Failure), System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Static);

            if (failureMethod is not null)
                return (TResponse)failureMethod.Invoke(null, [error])!;
        }

        return (TResponse)Result.Failure(error);
    }
}
