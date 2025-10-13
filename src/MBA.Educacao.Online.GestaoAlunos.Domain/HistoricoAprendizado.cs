namespace MBA.Educacao.Online.GestaoAlunos.Domain;

/// <summary>
/// Value Object: HistoricoAprendizado
/// Representa o histórico de aulas concluídas pelo aluno
/// </summary>
public class HistoricoAprendizado
{
    private readonly List<AulaConcluida> _aulasConcluidas;

    public HistoricoAprendizado()
    {
        _aulasConcluidas = new List<AulaConcluida>();
    }

    public IReadOnlyCollection<AulaConcluida> AulasConcluidas => _aulasConcluidas.AsReadOnly();

    public void RegistrarAulaConcluida(Guid cursoId, Guid aulaId)
    {
        // Não registra duplicado
        if (_aulasConcluidas.Any(a => a.AulaId == aulaId))
            return;

        var aulaConcluida = new AulaConcluida(cursoId, aulaId, DateTime.Now);
        _aulasConcluidas.Add(aulaConcluida);
    }

    public int ObterTotalAulasConcluidas() => _aulasConcluidas.Count;

    public int ObterTotalAulasConcluidasPorCurso(Guid cursoId)
    {
        return _aulasConcluidas.Count(a => a.CursoId == cursoId);
    }

    public bool AulaJaConcluida(Guid aulaId)
    {
        return _aulasConcluidas.Any(a => a.AulaId == aulaId);
    }
}

/// <summary>
/// Representa uma aula concluída no histórico
/// </summary>
public class AulaConcluida
{
    public AulaConcluida(Guid cursoId, Guid aulaId, DateTime dataAprendizado)
    {
        CursoId = cursoId;
        AulaId = aulaId;
        DataAprendizado = dataAprendizado;
    }

    public Guid CursoId { get; private set; }
    public Guid AulaId { get; private set; }
    public DateTime DataAprendizado { get; private set; }
}

