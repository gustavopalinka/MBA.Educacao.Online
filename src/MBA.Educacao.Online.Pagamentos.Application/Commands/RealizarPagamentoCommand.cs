using MBA.Educacao.Online.Core.Messages;

namespace MBA.Educacao.Online.Pagamentos.Application.Commands;

/// <summary>
/// Command para realizar um pagamento
/// Caso de Uso: Realização do Pagamento (do PDF)
/// </summary>
public class RealizarPagamentoCommand : Command
{
    public RealizarPagamentoCommand(Guid matriculaId, Guid alunoId, decimal valor,
                                    string numeroCartao, string nomeTitular, 
                                    string validade, string cvv)
    {
        MatriculaId = matriculaId;
        AlunoId = alunoId;
        Valor = valor;
        NumeroCartao = numeroCartao;
        NomeTitular = nomeTitular;
        Validade = validade;
        CVV = cvv;
    }

    public Guid MatriculaId { get; private set; }
    public Guid AlunoId { get; private set; }
    public decimal Valor { get; private set; }
    public string NumeroCartao { get; private set; }
    public string NomeTitular { get; private set; }
    public string Validade { get; private set; }
    public string CVV { get; private set; }

    public override bool EhValido()
    {
        return MatriculaId != Guid.Empty && 
               AlunoId != Guid.Empty && 
               Valor > 0 &&
               !string.IsNullOrEmpty(NumeroCartao) &&
               !string.IsNullOrEmpty(NomeTitular);
    }
}

