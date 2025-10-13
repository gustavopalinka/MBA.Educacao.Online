using MBA.Educacao.Online.Core.DomainObjects;

namespace MBA.Educacao.Online.GestaoConteudo.Domain;

/// <summary>
/// Aggregate Root: Curso
/// Representa um curso disponível na plataforma
/// </summary>
public class Curso : Entity, IAggregateRoot
{
    protected Curso() { }

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

    public string Nome { get; private set; }
    public string Descricao { get; private set; }
    public decimal Valor { get; private set; }
    public int CargaHoraria { get; private set; }
    public string PublicoAlvo { get; private set; }
    public string Objetivo { get; private set; }
    public string Requisitos { get; private set; }
    public DateTime DataCadastro { get; private set; }
    public ConteudoProgramatico ConteudoProgramatico { get; private set; }
    public bool Ativo { get; private set; }

    // Navegação
    public ICollection<Aula> Aulas { get; private set; }

    // Comportamentos
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

    // Validações
    private void ValidarCurso(string nome, string descricao, decimal valor, 
                             int cargaHoraria, string publicoAlvo, 
                             string objetivo, string requisitos)
    {
        Validacoes.ValidarSeVazio(nome, "O nome do curso não pode ser vazio");
        Validacoes.ValidarTamanho(nome, NomeMaxLength, "O nome do curso deve ter no máximo {0} caracteres");
        
        Validacoes.ValidarSeVazio(descricao, "A descrição do curso não pode ser vazia");
        Validacoes.ValidarTamanho(descricao, DescricaoMaxLength, "A descrição do curso deve ter no máximo {0} caracteres");
        
        Validacoes.ValidarSeMenorQue(valor, 0, "O valor do curso não pode ser negativo");
        Validacoes.ValidarSeMenorQue(cargaHoraria, 1, "A carga horária deve ser maior que zero");
        
        Validacoes.ValidarSeVazio(publicoAlvo, "O público alvo não pode ser vazio");
        Validacoes.ValidarTamanho(publicoAlvo, PublicoAlvoMaxLength, "O público alvo deve ter no máximo {0} caracteres");
        
        Validacoes.ValidarSeVazio(objetivo, "O objetivo não pode ser vazio");
        Validacoes.ValidarTamanho(objetivo, ObjetivoMaxLength, "O objetivo deve ter no máximo {0} caracteres");
        
        Validacoes.ValidarSeVazio(requisitos, "Os requisitos não podem ser vazios");
        Validacoes.ValidarTamanho(requisitos, RequisitosMaxLength, "Os requisitos devem ter no máximo {0} caracteres");
    }

    public override bool EhValido()
    {
        return !string.IsNullOrEmpty(Nome) && 
               !string.IsNullOrEmpty(Descricao) && 
               Valor >= 0 && 
               CargaHoraria > 0;
    }

    #region Constants
    public const int NomeMaxLength = 200;
    public const int DescricaoMaxLength = 1000;
    public const int PublicoAlvoMaxLength = 300;
    public const int ObjetivoMaxLength = 500;
    public const int RequisitosMaxLength = 500;
    #endregion
}

