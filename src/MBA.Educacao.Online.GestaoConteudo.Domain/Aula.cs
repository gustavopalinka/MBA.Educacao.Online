using MBA.Educacao.Online.Core.DomainObjects;

namespace MBA.Educacao.Online.GestaoConteudo.Domain;

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
    public virtual Curso Curso { get; private set; }

    public void AlterarEstado(bool ativo) => Ativo = ativo;

    public void AtualizarInformacoes(string codigo, string titulo, string descricao, int ordem)
    {
        ValidarAula(codigo, titulo, descricao, ordem);
        
        Codigo = codigo;
        Titulo = titulo;
        Descricao = descricao;
        Ordem = ordem;
    }

    private void ValidarAula(string codigo, string titulo, string descricao, int ordem)
    {
        Validacoes.ValidarSeVazio(codigo, "O código da aula não pode ser vazio");
        Validacoes.ValidarSeVazio(titulo, "O título da aula não pode ser vazio");
        Validacoes.ValidarSeVazio(descricao, "A descrição da aula não pode ser vazia");
        Validacoes.ValidarSeMenorQue(ordem, 1, "A ordem da aula deve ser maior que zero");
    }

    public override bool EhValido()
    {
        return !string.IsNullOrEmpty(Codigo) && 
               !string.IsNullOrEmpty(Titulo) && 
               Ordem > 0;
    }
}