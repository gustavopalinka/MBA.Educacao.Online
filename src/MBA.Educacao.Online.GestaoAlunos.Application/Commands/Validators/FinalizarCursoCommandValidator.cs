using FluentValidation;

namespace MBA.Educacao.Online.GestaoAlunos.Application.Commands.Validators;

public class FinalizarCursoCommandValidator : AbstractValidator<FinalizarCursoCommand>
{
    public FinalizarCursoCommandValidator()
    {
        RuleFor(c => c.AlunoId)
            .NotEmpty()
            .WithMessage("O identificador do aluno é obrigatório.");

        RuleFor(c => c.CursoId)
            .NotEmpty()
            .WithMessage("O identificador do curso é obrigatório.");

        RuleFor(c => c.MatriculaId)
            .NotEmpty()
            .WithMessage("O identificador da matrícula é obrigatório.");
    }
}

