using MBA.Educacao.Online.Core.Messages;

namespace MBA.Educacao.Online.Pagamentos.Application.Commands;

/// <summary>
/// Command para confirmar um pagamento
/// (simulação de callback de gateway de pagamento)
/// </summary>
public class ConfirmarPagamentoCommand : Command
{
    public ConfirmarPagamentoCommand(Guid pagamentoId)
    {
        PagamentoId = pagamentoId;
    }

    public Guid PagamentoId { get; private set; }

    public override bool EhValido()
    {
        return PagamentoId != Guid.Empty;
    }
}

