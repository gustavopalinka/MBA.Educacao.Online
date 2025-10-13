using MBA.Educacao.Online.Core.DomainObjects;

namespace MBA.Educacao.Online.GestaoAlunos.Domain;

/// <summary>
/// Entity: Certificado
/// Representa um certificado de conclusão de curso
/// </summary>
public class Certificado : Entity
{
    protected Certificado() { }

    public Certificado(Guid alunoId, Guid cursoId, DateTime dataEmissao, string codigo)
    {
        Validacoes.ValidarSeVazio(codigo, "O código do certificado não pode ser vazio");
        Validacoes.ValidarTamanho(codigo, CodigoMaxLength, 
            $"O código do certificado deve ter no máximo {CodigoMaxLength} caracteres");
        
        AlunoId = alunoId;
        CursoId = cursoId;
        DataEmissao = dataEmissao;
        Codigo = codigo;
        Valido = true;
    }

    public Guid AlunoId { get; private set; }
    public Guid CursoId { get; private set; }
    public DateTime DataEmissao { get; private set; }
    public string Codigo { get; private set; }
    public bool Valido { get; private set; }

    // Navegação
    public virtual Aluno Aluno { get; private set; }

    // Comportamentos
    public void Invalidar() => Valido = false;

    public void Revalidar() => Valido = true;

    #region Constants
    public const int CodigoMaxLength = 50;
    #endregion
}

