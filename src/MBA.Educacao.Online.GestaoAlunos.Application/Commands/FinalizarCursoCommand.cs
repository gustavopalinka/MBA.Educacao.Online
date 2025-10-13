using MBA.Educacao.Online.Core.Messages;

namespace MBA.Educacao.Online.GestaoAlunos.Application.Commands;

/// <summary>
/// Command para finalizar um curso e gerar certificado
/// Caso de Uso: Finalização do Curso (do PDF)
/// </summary>
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
        return AlunoId != Guid.Empty && CursoId != Guid.Empty && MatriculaId != Guid.Empty;
    }
}

