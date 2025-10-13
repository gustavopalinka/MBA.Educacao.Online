using FluentValidation;
using MBA.Educacao.Online.GestaoConteudo.Application.Commands;

namespace MBA.Educacao.Online.GestaoConteudo.Application.Commands;

public class CriarCursoCommandValidator : AbstractValidator<CriarCursoCommand>
{
    public CriarCursoCommandValidator()
    {
        RuleFor(c => c.Nome)
            .NotEmpty().WithMessage("O nome do curso é obrigatório")
            .MaximumLength(200)
            .WithMessage("O nome do curso deve ter no máximo 200 caracteres");

        RuleFor(c => c.Descricao)
            .NotEmpty().WithMessage("A descrição do curso é obrigatória")
            .MaximumLength(1000)
            .WithMessage("A descrição do curso deve ter no máximo 1000 caracteres");

        RuleFor(c => c.Valor)
            .GreaterThanOrEqualTo(0).WithMessage("O valor do curso não pode ser negativo");

        RuleFor(c => c.CargaHoraria)
            .GreaterThan(0).WithMessage("A carga horária deve ser maior que zero");

        RuleFor(c => c.PublicoAlvo)
            .NotEmpty().WithMessage("O público alvo é obrigatório")
            .MaximumLength(300)
            .WithMessage("O público alvo deve ter no máximo 300 caracteres");

        RuleFor(c => c.Objetivo)
            .NotEmpty().WithMessage("O objetivo é obrigatório")
            .MaximumLength(500)
            .WithMessage("O objetivo deve ter no máximo 500 caracteres");

        RuleFor(c => c.Requisitos)
            .NotEmpty().WithMessage("Os requisitos são obrigatórios")
            .MaximumLength(500)
            .WithMessage("Os requisitos devem ter no máximo 500 caracteres");

        RuleFor(c => c.ConteudoProgramatico)
            .NotEmpty().WithMessage("O conteúdo programático é obrigatório")
            .MaximumLength(1000)
            .WithMessage("O conteúdo programático deve ter no máximo 1000 caracteres");
    }
}