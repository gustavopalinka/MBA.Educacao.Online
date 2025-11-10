using FluentValidation;

namespace MBA.Educacao.Online.Pagamentos.Application.Commands.Validators;

public class RealizarPagamentoCommandValidator : AbstractValidator<RealizarPagamentoCommand>
{
    public RealizarPagamentoCommandValidator()
    {
        RuleFor(c => c.MatriculaId)
            .NotEmpty()
            .WithMessage("A matrícula é obrigatória.");

        RuleFor(c => c.AlunoId)
            .NotEmpty()
            .WithMessage("O aluno é obrigatório.");

        RuleFor(c => c.Valor)
            .GreaterThan(0)
            .WithMessage("O valor do pagamento deve ser maior que zero.");

        RuleFor(c => c.NumeroCartao)
            .NotEmpty().WithMessage("O número do cartão é obrigatório.")
            .Length(13, 19).WithMessage("O número do cartão deve conter entre 13 e 19 dígitos.")
            .Matches("^[0-9]+$").WithMessage("O número do cartão deve conter apenas dígitos.");

        RuleFor(c => c.NomeTitular)
            .NotEmpty()
            .WithMessage("O nome do titular é obrigatório.");

        RuleFor(c => c.Validade)
            .NotEmpty()
            .WithMessage("A validade do cartão é obrigatória.");

        RuleFor(c => c.CVV)
            .Length(3, 4)
            .WithMessage("O CVV deve possuir entre 3 e 4 dígitos.");
    }
}

