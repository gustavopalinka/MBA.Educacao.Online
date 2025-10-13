using FluentValidation;
using MBA.Educacao.Online.GestaoConteudo.Application.Commands;
using MBA.Educacao.Online.GestaoConteudo.Domain;

namespace MBA.Educacao.Online.GestaoConteudo.Application.Commands;

/// <summary>
/// Validador para AdicionarAulaCommand
/// </summary>
public class AdicionarAulaCommandValidator : AbstractValidator<AdicionarAulaCommand>
{
    public AdicionarAulaCommandValidator()
    {
        RuleFor(c => c.CursoId)
            .NotEqual(Guid.Empty).WithMessage("O ID do curso é obrigatório");

        RuleFor(c => c.Codigo)
            .NotEmpty().WithMessage("O código da aula é obrigatório")
            .MaximumLength(Aula.CodigoMaxLength)
            .WithMessage($"O código da aula deve ter no máximo {Aula.CodigoMaxLength} caracteres");

        RuleFor(c => c.Titulo)
            .NotEmpty().WithMessage("O título da aula é obrigatório")
            .MaximumLength(Aula.TituloMaxLength)
            .WithMessage($"O título da aula deve ter no máximo {Aula.TituloMaxLength} caracteres");

        RuleFor(c => c.Descricao)
            .NotEmpty().WithMessage("A descrição da aula é obrigatória")
            .MaximumLength(Aula.DescricaoMaxLength)
            .WithMessage($"A descrição da aula deve ter no máximo {Aula.DescricaoMaxLength} caracteres");

        RuleFor(c => c.Ordem)
            .GreaterThan(0).WithMessage("A ordem da aula deve ser maior que zero");
    }
}

