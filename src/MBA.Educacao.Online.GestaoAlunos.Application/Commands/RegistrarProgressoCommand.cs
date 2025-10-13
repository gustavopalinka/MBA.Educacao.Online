using MBA.Educacao.Online.Core.Messages;

namespace MBA.Educacao.Online.GestaoAlunos.Application.Commands;

/// <summary>
/// Command para registrar o progresso do aluno em uma aula
/// Caso de Uso: Realização da Aula (do PDF)
/// </summary>
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
        return AlunoId != Guid.Empty && CursoId != Guid.Empty && AulaId != Guid.Empty;
    }
}

