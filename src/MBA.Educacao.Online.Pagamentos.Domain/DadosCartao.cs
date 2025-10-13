using MBA.Educacao.Online.Core.DomainObjects;

namespace MBA.Educacao.Online.Pagamentos.Domain;

/// <summary>
/// Value Object: DadosCartao
/// Representa os dados de um cartão de crédito (sensíveis)
/// </summary>
public class DadosCartao
{
    public DadosCartao(string numeroCartao, string nomeTitular, string validade, string cvv)
    {
        ValidarDadosCartao(numeroCartao, nomeTitular, validade, cvv);
        
        NumeroCartao = MascararNumeroCartao(numeroCartao);
        NumeroCartaoCompleto = numeroCartao; // Em produção, deve ser criptografado!
        NomeTitular = nomeTitular;
        Validade = validade;
        CVV = cvv; // Em produção, NÃO deve ser armazenado!
    }

    public string NumeroCartao { get; private set; } // Mascarado: **** **** **** 1234
    private string NumeroCartaoCompleto { get; set; } // Somente para processamento
    public string NomeTitular { get; private set; }
    public string Validade { get; private set; }
    private string CVV { get; set; } // Não expor publicamente

    private void ValidarDadosCartao(string numeroCartao, string nomeTitular, string validade, string cvv)
    {
        Validacoes.ValidarSeVazio(numeroCartao, "O número do cartão é obrigatório");
        Validacoes.ValidarSeVazio(nomeTitular, "O nome do titular é obrigatório");
        Validacoes.ValidarSeVazio(validade, "A validade do cartão é obrigatória");
        Validacoes.ValidarSeVazio(cvv, "O CVV é obrigatório");

        // Validação básica de comprimento
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

