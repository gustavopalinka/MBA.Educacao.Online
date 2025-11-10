using FluentValidation;

namespace MBA.Educacao.Online.GestaoAlunos.Application.Commands.Validators;

public class RegistrarProgressoCommandValidator : AbstractValidator<RegistrarProgressoCommand>
{
    public RegistrarProgressoCommandValidator()
    {
        RuleFor(c => c.AlunoId)
            .NotEmpty()
            .WithMessage("O identificador do aluno é obrigatório.");

        RuleFor(c => c.CursoId)
            .NotEmpty()
            .WithMessage("O identificador do curso é obrigatório.");

        RuleFor(c => c.AulaId)
            .NotEmpty()
            .WithMessage("O identificador da aula é obrigatório.");
    }
}

