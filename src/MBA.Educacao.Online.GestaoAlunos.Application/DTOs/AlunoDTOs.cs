namespace MBA.Educacao.Online.GestaoAlunos.Application.DTOs;

public class MatriculaDTO
{
    public Guid Id { get; set; }
    public Guid CursoId { get; set; }
    public DateTime DataMatricula { get; set; }
    public DateTime DataValidade { get; set; }
    public DateTime? DataConclusao { get; set; }
    public string Status { get; set; } = string.Empty;
}

public class CertificadoDTO
{
    public Guid Id { get; set; }
    public Guid CursoId { get; set; }
    public DateTime DataEmissao { get; set; }
    public string Codigo { get; set; } = string.Empty;
}

public class ProgressoCursoDTO
{
    public Guid CursoId { get; set; }
    public int TotalAulas { get; set; }
    public int AulasConcluidas { get; set; }
    public decimal PercentualConcluido { get; set; }
    public string StatusMatricula { get; set; } = string.Empty;
}

