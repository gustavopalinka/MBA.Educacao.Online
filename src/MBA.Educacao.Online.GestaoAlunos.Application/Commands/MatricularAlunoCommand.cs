using FluentValidation.Results;
using MBA.Educacao.Online.Core.Messages;
using MBA.Educacao.Online.GestaoAlunos.Application.Commands.Validators;

namespace MBA.Educacao.Online.GestaoAlunos.Application.Commands;

public class MatricularAlunoCommand : Command
{
    public MatricularAlunoCommand(Guid alunoId, Guid cursoId)
    {
        AlunoId = alunoId;
        CursoId = cursoId;
    }

    public Guid AlunoId { get; private set; }
    public Guid CursoId { get; private set; }

    public override bool EhValido()
    {
        ValidationResult = new MatricularAlunoCommandValidator().Validate(this);
        return ValidationResult.IsValid;
    }
}