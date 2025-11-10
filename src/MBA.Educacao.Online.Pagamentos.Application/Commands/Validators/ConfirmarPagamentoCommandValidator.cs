using FluentValidation;

namespace MBA.Educacao.Online.Pagamentos.Application.Commands.Validators;

public class ConfirmarPagamentoCommandValidator : AbstractValidator<ConfirmarPagamentoCommand>
{
    public ConfirmarPagamentoCommandValidator()
    {
        RuleFor(c => c.PagamentoId)
            .NotEmpty()
            .WithMessage("O identificador do pagamento é obrigatório.");
    }
}

