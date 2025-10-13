namespace MBA.Educacao.Online.GestaoConteudo.Application.DTOs;

/// <summary>
/// DTO para retorno de dados de Curso
/// </summary>
public class CursoDTO
{
    public Guid Id { get; set; }
    public string Nome { get; set; } = string.Empty;
    public string Descricao { get; set; } = string.Empty;
    public decimal Valor { get; set; }
    public int CargaHoraria { get; set; }
    public string PublicoAlvo { get; set; } = string.Empty;
    public string Objetivo { get; set; } = string.Empty;
    public string Requisitos { get; set; } = string.Empty;
    public string ConteudoProgramatico { get; set; } = string.Empty;
    public bool Ativo { get; set; }
    public DateTime DataCadastro { get; set; }
    public List<AulaDTO> Aulas { get; set; } = new();
}

/// <summary>
/// DTO para retorno de dados de Aula
/// </summary>
public class AulaDTO
{
    public Guid Id { get; set; }
    public string Codigo { get; set; } = string.Empty;
    public string Titulo { get; set; } = string.Empty;
    public string Descricao { get; set; } = string.Empty;
    public int Ordem { get; set; }
    public bool Ativo { get; set; }
    public DateTime DataCadastro { get; set; }
}

