using FluentValidation;

namespace CashFlow.Consolidado.Application.Queries.ObterConsolidadoPorPeriodo;

public class ObterConsolidadoPorPeriodoValidator
    : AbstractValidator<ObterConsolidadoPorPeriodoQuery>
{
    public ObterConsolidadoPorPeriodoValidator()
    {
        RuleFor(x => x.DataInicio)
            .NotEmpty()
            .WithMessage("A data de início é obrigatória.");

        RuleFor(x => x.DataFim)
            .NotEmpty()
            .WithMessage("A data de fim é obrigatória.");

        RuleFor(x => x)
            .Must(x => x.DataInicio <= x.DataFim)
            .WithMessage("A data de início deve ser anterior ou igual a data de fim.");
    }
}
