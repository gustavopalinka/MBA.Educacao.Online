using MBA.Educacao.Online.Core.DomainObjects;

namespace MBA.Educacao.Online.GestaoConteudo.Domain;

/// <summary>
/// Entity: Aula
/// Representa uma aula de um curso
/// </summary>
public class Aula : Entity
{
    protected Aula() { }

    public Aula(string codigo, string titulo, string descricao, int ordem, Guid cursoId)
    {
        ValidarAula(codigo, titulo, descricao, ordem);
        
        Codigo = codigo;
        Titulo = titulo;
        Descricao = descricao;
        Ordem = ordem;
        CursoId = cursoId;
        DataCadastro = DateTime.Now;
        Ativo = true;
    }

    public string Codigo { get; private set; }
    public string Titulo { get; private set; }
    public string Descricao { get; private set; }
    public int Ordem { get; private set; }
    public Guid CursoId { get; private set; }
    public DateTime DataCadastro { get; private set; }
    public bool Ativo { get; private set; }
    
    // Navegação
    public virtual Curso Curso { get; private set; }

    // Comportamentos
    public void AlterarEstado(bool ativo) => Ativo = ativo;

    public void AtualizarInformacoes(string codigo, string titulo, string descricao, int ordem)
    {
        ValidarAula(codigo, titulo, descricao, ordem);
        
        Codigo = codigo;
        Titulo = titulo;
        Descricao = descricao;
        Ordem = ordem;
    }

    // Validações
    private void ValidarAula(string codigo, string titulo, string descricao, int ordem)
    {
        Validacoes.ValidarSeVazio(codigo, "O código da aula não pode ser vazio");
        Validacoes.ValidarTamanho(codigo, CodigoMaxLength, $"O código da aula deve ter no máximo {CodigoMaxLength} caracteres");
        
        Validacoes.ValidarSeVazio(titulo, "O título da aula não pode ser vazio");
        Validacoes.ValidarTamanho(titulo, TituloMaxLength, $"O título da aula deve ter no máximo {TituloMaxLength} caracteres");
        
        Validacoes.ValidarSeVazio(descricao, "A descrição da aula não pode ser vazia");
        Validacoes.ValidarTamanho(descricao, DescricaoMaxLength, $"A descrição da aula deve ter no máximo {DescricaoMaxLength} caracteres");
        
        Validacoes.ValidarSeMenorQue(ordem, 1, "A ordem da aula deve ser maior que zero");
    }

    public override bool EhValido()
    {
        return !string.IsNullOrEmpty(Codigo) && 
               !string.IsNullOrEmpty(Titulo) && 
               Ordem > 0;
    }

    #region Constants
    public const int CodigoMaxLength = 20;
    public const int TituloMaxLength = 200;
    public const int DescricaoMaxLength = 500;
    #endregion
}

