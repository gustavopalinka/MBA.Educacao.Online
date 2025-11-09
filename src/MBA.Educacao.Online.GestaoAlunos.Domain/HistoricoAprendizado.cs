namespace MBA.Educacao.Online.GestaoAlunos.Domain;

public class HistoricoAprendizado
{
    private readonly List<AulaConcluida> _aulasConcluidas;

    public HistoricoAprendizado()
    {
        _aulasConcluidas = new List<AulaConcluida>();
    }

    public IReadOnlyCollection<AulaConcluida> AulasConcluidas => _aulasConcluidas.AsReadOnly();
    public List<AulaConcluida> AulasConcluidasEf
    {
        get => _aulasConcluidas;
        private set
        {
            _aulasConcluidas.Clear();
            if (value is null) return;
            _aulasConcluidas.AddRange(value);
        }
    }

    public void RegistrarAulaConcluida(Guid cursoId, Guid aulaId)
    {
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

    public DateTime? ObterDataConclusao(Guid aulaId)
    {
        return _aulasConcluidas.FirstOrDefault(a => a.AulaId == aulaId)?.DataAprendizado;
    }
}

public class AulaConcluida
{
    protected AulaConcluida() { }

    public AulaConcluida(Guid cursoId, Guid aulaId, DateTime dataAprendizado)
    {
        Id = Guid.NewGuid();
        CursoId = cursoId;
        AulaId = aulaId;
        DataAprendizado = dataAprendizado;
    }

    public Guid Id { get; private set; }
    public Guid CursoId { get; private set; }
    public Guid AulaId { get; private set; }
    public DateTime DataAprendizado { get; private set; }
}