namespace MBA.Educacao.Online.GestaoAlunos.Application.DTOs;

public class MatriculaDTO
{
    public Guid Id { get; set; }
    public Guid CursoId { get; set; }
    public string NomeCurso { get; set; } = string.Empty;
    public DateTime DataMatricula { get; set; }
    public DateTime DataValidade { get; set; }
    public DateTime? DataConclusao { get; set; }
    public string Status { get; set; } = string.Empty;
}

public class CertificadoDTO
{
    public Guid Id { get; set; }
    public Guid CursoId { get; set; }
    public string NomeCurso { get; set; } = string.Empty;
    public DateTime DataEmissao { get; set; }
    public string CodigoCertificado { get; set; } = string.Empty;
}

public class ProgressoCursoDTO
{
    public Guid CursoId { get; set; }
    public string NomeCurso { get; set; } = string.Empty;
    public int TotalAulas { get; set; }
    public int AulasConcluidas { get; set; }
    public decimal PercentualConcluido { get; set; }
    public string StatusMatricula { get; set; } = string.Empty;
    public List<AulaProgressoDTO> Aulas { get; set; } = new();
}

public class AulaProgressoDTO
{
    public Guid AulaId { get; set; }
    public string TituloAula { get; set; } = string.Empty;
    public bool Concluida { get; set; }
    public DateTime? DataConclusao { get; set; }
}

