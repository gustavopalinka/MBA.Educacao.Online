using FluentValidation;
using MBA.Educacao.Online.GestaoConteudo.Application.Commands;
using MBA.Educacao.Online.GestaoConteudo.Domain;

namespace MBA.Educacao.Online.GestaoConteudo.Application.Commands;

/// <summary>
/// Validador para CriarCursoCommand
/// </summary>
public class CriarCursoCommandValidator : AbstractValidator<CriarCursoCommand>
{
    public CriarCursoCommandValidator()
    {
        RuleFor(c => c.Nome)
            .NotEmpty().WithMessage("O nome do curso é obrigatório")
            .MaximumLength(Curso.NomeMaxLength)
            .WithMessage($"O nome do curso deve ter no máximo {Curso.NomeMaxLength} caracteres");

        RuleFor(c => c.Descricao)
            .NotEmpty().WithMessage("A descrição do curso é obrigatória")
            .MaximumLength(Curso.DescricaoMaxLength)
            .WithMessage($"A descrição do curso deve ter no máximo {Curso.DescricaoMaxLength} caracteres");

        RuleFor(c => c.Valor)
            .GreaterThanOrEqualTo(0).WithMessage("O valor do curso não pode ser negativo");

        RuleFor(c => c.CargaHoraria)
            .GreaterThan(0).WithMessage("A carga horária deve ser maior que zero");

        RuleFor(c => c.PublicoAlvo)
            .NotEmpty().WithMessage("O público alvo é obrigatório")
            .MaximumLength(Curso.PublicoAlvoMaxLength)
            .WithMessage($"O público alvo deve ter no máximo {Curso.PublicoAlvoMaxLength} caracteres");

        RuleFor(c => c.Objetivo)
            .NotEmpty().WithMessage("O objetivo é obrigatório")
            .MaximumLength(Curso.ObjetivoMaxLength)
            .WithMessage($"O objetivo deve ter no máximo {Curso.ObjetivoMaxLength} caracteres");

        RuleFor(c => c.Requisitos)
            .NotEmpty().WithMessage("Os requisitos são obrigatórios")
            .MaximumLength(Curso.RequisitosMaxLength)
            .WithMessage($"Os requisitos devem ter no máximo {Curso.RequisitosMaxLength} caracteres");

        RuleFor(c => c.ConteudoProgramatico)
            .NotEmpty().WithMessage("O conteúdo programático é obrigatório")
            .MaximumLength(ConteudoProgramatico.DescricaoMaxLength)
            .WithMessage($"O conteúdo programático deve ter no máximo {ConteudoProgramatico.DescricaoMaxLength} caracteres");
    }
}

