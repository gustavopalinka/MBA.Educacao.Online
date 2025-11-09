using FluentValidation.Results;
using MBA.Educacao.Online.Core.Messages;
using MBA.Educacao.Online.GestaoAlunos.Application.Commands.Validators;

namespace MBA.Educacao.Online.GestaoAlunos.Application.Commands;

public class RegistrarProgressoCommand : Command
{
    public RegistrarProgressoCommand(Guid alunoId, Guid cursoId, Guid aulaId)
    {
        AlunoId = alunoId;
        CursoId = cursoId;
        AulaId = aulaId;
    }

    public Guid AlunoId { get; private set; }
    public Guid CursoId { get; private set; }
    public Guid AulaId { get; private set; }

    public override bool EhValido()
    {
        ValidationResult = new RegistrarProgressoCommandValidator().Validate(this);
        return ValidationResult.IsValid;
    }
}