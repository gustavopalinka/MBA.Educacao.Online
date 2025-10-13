using MBA.Educacao.Online.Core.DomainObjects;

namespace MBA.Educacao.Online.Pagamentos.Domain;

public class DadosCartao
{
    protected DadosCartao() { }

    public DadosCartao(string numeroCartao, string nomeTitular, string validade, string cvv)
    {
        ValidarDadosCartao(numeroCartao, nomeTitular, validade, cvv);
        
        NumeroCartao = MascararNumeroCartao(numeroCartao);
        NumeroCartaoCompleto = numeroCartao;
        NomeTitular = nomeTitular;
        Validade = validade;
        CVV = cvv;
    }

    public string NumeroCartao { get; private set; } = string.Empty;
    private string NumeroCartaoCompleto { get; set; } = string.Empty;
    public string NomeTitular { get; private set; } = string.Empty;
    public string Validade { get; private set; } = string.Empty;
    public string CVV { get; private set; } = string.Empty;

    private void ValidarDadosCartao(string numeroCartao, string nomeTitular, string validade, string cvv)
    {
        Validacoes.ValidarSeVazio(numeroCartao, "O número do cartão é obrigatório");
        Validacoes.ValidarSeVazio(nomeTitular, "O nome do titular é obrigatório");
        Validacoes.ValidarSeVazio(validade, "A validade do cartão é obrigatória");
        Validacoes.ValidarSeVazio(cvv, "O CVV é obrigatório");

        if (numeroCartao.Length < 13 || numeroCartao.Length > 19)
            throw new DomainException("Número do cartão inválido");

        if (cvv.Length < 3 || cvv.Length > 4)
            throw new DomainException("CVV inválido");
    }

    private string MascararNumeroCartao(string numero)
    {
        if (numero.Length <= 4)
            return numero;

        var ultimosDigitos = numero.Substring(numero.Length - 4);
        return $"**** **** **** {ultimosDigitos}";
    }

    public override bool Equals(object? obj)
    {
        if (obj is not DadosCartao other) return false;
        return NumeroCartao == other.NumeroCartao;
    }

    public override int GetHashCode()
    {
        return NumeroCartao.GetHashCode();
    }
}