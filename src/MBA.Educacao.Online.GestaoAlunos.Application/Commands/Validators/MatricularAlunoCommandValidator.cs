using FluentValidation;

namespace MBA.Educacao.Online.GestaoAlunos.Application.Commands.Validators;

public class MatricularAlunoCommandValidator : AbstractValidator<MatricularAlunoCommand>
{
    public MatricularAlunoCommandValidator()
    {
        RuleFor(c => c.AlunoId)
            .NotEmpty()
            .WithMessage("O identificador do aluno é obrigatório.");

        RuleFor(c => c.CursoId)
            .NotEmpty()
            .WithMessage("O identificador do curso é obrigatório.");
    }
}

