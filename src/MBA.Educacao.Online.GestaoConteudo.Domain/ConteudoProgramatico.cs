using MBA.Educacao.Online.Core.DomainObjects;

namespace MBA.Educacao.Online.GestaoConteudo.Domain;

public class ConteudoProgramatico
{
    protected ConteudoProgramatico() { }

    public ConteudoProgramatico(string conteudoDescricao, int revisao, DateTime dataRevisao)
    {
        ValidarConteudo(conteudoDescricao, revisao);
        
        ConteudoDescricao = conteudoDescricao;
        Revisao = revisao;
        DataRevisao = dataRevisao;
    }

    public string ConteudoDescricao { get; private set; } = string.Empty;
    public int Revisao { get; private set; }
    public DateTime DataRevisao { get; private set; }

    public ConteudoProgramatico NovaRevisao(string novaDescricao)
    {
        return new ConteudoProgramatico(novaDescricao, Revisao + 1, DateTime.Now);
    }

    private void ValidarConteudo(string conteudoDescricao, int revisao)
    {
        Validacoes.ValidarSeVazio(conteudoDescricao, "A descrição do conteúdo programático não pode ser vazia");
        Validacoes.ValidarSeMenorQue(revisao, 0, "A revisão não pode ser negativa");
    }

    public override bool Equals(object? obj)
    {
        if (obj is not ConteudoProgramatico other) return false;
        
        return ConteudoDescricao == other.ConteudoDescricao &&
               Revisao == other.Revisao &&
               DataRevisao.Date == other.DataRevisao.Date;
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(ConteudoDescricao, Revisao, DataRevisao.Date);
    }
}