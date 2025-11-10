using MBA.Educacao.Online.Core.DomainObjects;

namespace MBA.Educacao.Online.GestaoConteudo.Domain;

public class Curso : Entity, IAggregateRoot
{
    protected Curso()
    {
        Nome = string.Empty;
        Descricao = string.Empty;
        PublicoAlvo = string.Empty;
        Objetivo = string.Empty;
        Requisitos = string.Empty;
        ConteudoProgramatico = null!;
        Aulas = new List<Aula>();
    }

    public Curso(string nome, string descricao, decimal valor, int cargaHoraria, 
                 string publicoAlvo, string objetivo, string requisitos, 
                 ConteudoProgramatico conteudoProgramatico)
    {
        ValidarCurso(nome, descricao, valor, cargaHoraria, publicoAlvo, objetivo, requisitos);
        
        Nome = nome;
        Descricao = descricao;
        Valor = valor;
        CargaHoraria = cargaHoraria;
        PublicoAlvo = publicoAlvo;
        Objetivo = objetivo;
        Requisitos = requisitos;
        ConteudoProgramatico = conteudoProgramatico;
        DataCadastro = DateTime.Now;
        Ativo = true;
        Aulas = new List<Aula>();
    }

    public string Nome { get; private set; } = string.Empty;
    public string Descricao { get; private set; } = string.Empty;
    public decimal Valor { get; private set; }
    public int CargaHoraria { get; private set; }
    public string PublicoAlvo { get; private set; } = string.Empty;
    public string Objetivo { get; private set; } = string.Empty;
    public string Requisitos { get; private set; } = string.Empty;
    public DateTime DataCadastro { get; private set; }
    public ConteudoProgramatico ConteudoProgramatico { get; private set; } = null!;
    public bool Ativo { get; private set; }
    public ICollection<Aula> Aulas { get; private set; } = new List<Aula>();

    public void AlterarEstado(bool ativo) => Ativo = ativo;

    public void AlterarConteudoProgramatico(ConteudoProgramatico conteudoProgramatico)
    {
        Validacoes.ValidarSeNulo(conteudoProgramatico, "Conteúdo programático não pode ser nulo");
        ConteudoProgramatico = conteudoProgramatico;
    }

    public void AdicionarAula(Aula aula)
    {
        Validacoes.ValidarSeNulo(aula, "Aula não pode ser nula");
        Aulas.Add(aula);
    }

    public void RemoverAula(Aula aula)
    {
        Validacoes.ValidarSeNulo(aula, "Aula não pode ser nula");
        Aulas.Remove(aula);
    }

    public void AtualizarInformacoes(string nome, string descricao, decimal valor, 
                                     int cargaHoraria, string publicoAlvo, 
                                     string objetivo, string requisitos)
    {
        ValidarCurso(nome, descricao, valor, cargaHoraria, publicoAlvo, objetivo, requisitos);
        
        Nome = nome;
        Descricao = descricao;
        Valor = valor;
        CargaHoraria = cargaHoraria;
        PublicoAlvo = publicoAlvo;
        Objetivo = objetivo;
        Requisitos = requisitos;
    }

    private void ValidarCurso(string nome, string descricao, decimal valor, 
                             int cargaHoraria, string publicoAlvo, 
                             string objetivo, string requisitos)
    {
        Validacoes.ValidarSeVazio(nome, "O nome do curso não pode ser vazio");
        Validacoes.ValidarSeVazio(descricao, "A descrição do curso não pode ser vazia");
        Validacoes.ValidarSeMenorQue(valor, 0, "O valor do curso não pode ser negativo");
        Validacoes.ValidarSeMenorQue(cargaHoraria, 1, "A carga horária deve ser maior que zero");
        Validacoes.ValidarSeVazio(publicoAlvo, "O público alvo não pode ser vazio");
        Validacoes.ValidarSeVazio(objetivo, "O objetivo não pode ser vazio");
        Validacoes.ValidarSeVazio(requisitos, "Os requisitos não podem ser vazios");
    }

    public override bool EhValido()
    {
        return !string.IsNullOrEmpty(Nome) && 
               !string.IsNullOrEmpty(Descricao) && 
               Valor >= 0 && 
               CargaHoraria > 0;
    }
}