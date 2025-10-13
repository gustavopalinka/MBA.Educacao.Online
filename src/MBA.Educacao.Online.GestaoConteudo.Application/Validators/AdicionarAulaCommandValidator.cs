using FluentValidation;
using MBA.Educacao.Online.GestaoConteudo.Application.Commands;

namespace MBA.Educacao.Online.GestaoConteudo.Application.Commands;

public class AdicionarAulaCommandValidator : AbstractValidator<AdicionarAulaCommand>
{
    public AdicionarAulaCommandValidator()
    {
        RuleFor(c => c.CursoId)
            .NotEqual(Guid.Empty).WithMessage("O ID do curso é obrigatório");

        RuleFor(c => c.Codigo)
            .NotEmpty().WithMessage("O código da aula é obrigatório")
            .MaximumLength(20)
            .WithMessage("O código da aula deve ter no máximo 20 caracteres");

        RuleFor(c => c.Titulo)
            .NotEmpty().WithMessage("O título da aula é obrigatório")
            .MaximumLength(200)
            .WithMessage("O título da aula deve ter no máximo 200 caracteres");

        RuleFor(c => c.Descricao)
            .NotEmpty().WithMessage("A descrição da aula é obrigatória")
            .MaximumLength(500)
            .WithMessage("A descrição da aula deve ter no máximo 500 caracteres");

        RuleFor(c => c.Ordem)
            .GreaterThan(0).WithMessage("A ordem da aula deve ser maior que zero");
    }
}