using FluentValidation;

namespace CashFlow.Lancamentos.Application.Commands.AtualizarLancamento;

public class AtualizarLancamentoValidator : AbstractValidator<AtualizarLancamentoCommand>
{
    public AtualizarLancamentoValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty()
            .WithMessage("O Id é obrigatório.");

        RuleFor(x => x.Valor)
            .GreaterThan(0)
            .WithMessage("O valor deve ser maior que zero.");

        RuleFor(x => x.Descricao)
            .NotEmpty()
            .WithMessage("A descrição é obrigatória.")
            .MaximumLength(200)
            .WithMessage("A descrição não pode ter mais que 200 caracteres.");

        RuleFor(x => x.Tipo)
            .IsInEnum()
            .WithMessage("O tipo de lançamento é inválido.");

        RuleFor(x => x.Data)
            .NotEmpty()
            .WithMessage("A data é obrigatória.");
    }
}
