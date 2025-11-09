using FluentValidation.Results;
using MBA.Educacao.Online.Core.Messages;
using MBA.Educacao.Online.GestaoAlunos.Application.Commands.Validators;

namespace MBA.Educacao.Online.GestaoAlunos.Application.Commands;

public class FinalizarCursoCommand : Command
{
    public FinalizarCursoCommand(Guid alunoId, Guid cursoId, Guid matriculaId)
    {
        AlunoId = alunoId;
        CursoId = cursoId;
        MatriculaId = matriculaId;
    }

    public Guid AlunoId { get; private set; }
    public Guid CursoId { get; private set; }
    public Guid MatriculaId { get; private set; }

    public override bool EhValido()
    {
        ValidationResult = new FinalizarCursoCommandValidator().Validate(this);
        return ValidationResult.IsValid;
    }
}