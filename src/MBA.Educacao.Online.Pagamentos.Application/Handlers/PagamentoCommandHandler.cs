using MBA.Educacao.Online.Pagamentos.Application.Commands;
using MBA.Educacao.Online.Pagamentos.Domain;
using MediatR;

namespace MBA.Educacao.Online.Pagamentos.Application.Handlers;

/// <summary>
/// Handler para Commands relacionados a Pagamento
/// Implementa os casos de uso de pagamento
/// </summary>
public class PagamentoCommandHandler :
    IRequestHandler<RealizarPagamentoCommand, bool>,
    IRequestHandler<ConfirmarPagamentoCommand, bool>
{
    private readonly IPagamentoRepository _pagamentoRepository;

    public PagamentoCommandHandler(IPagamentoRepository pagamentoRepository)
    {
        _pagamentoRepository = pagamentoRepository;
    }

    /// <summary>
    /// Handler para realizar um pagamento
    /// Caso de Uso: Realização do Pagamento
    /// </summary>
    public async Task<bool> Handle(RealizarPagamentoCommand request, CancellationToken cancellationToken)
    {
        if (!request.EhValido())
        {
            return false;
        }

        // Criar Value Object DadosCartao
        var dadosCartao = new DadosCartao(
            request.NumeroCartao,
            request.NomeTitular,
            request.Validade,
            request.CVV
        );

        // Criar Aggregate Root Pagamento
        var pagamento = new Pagamento(
            request.MatriculaId,
            request.AlunoId,
            request.Valor,
            dadosCartao
        );

        // Simular processamento de pagamento
        // Em produção, aqui seria a integração com gateway de pagamento
        var pagamentoAprovado = SimularProcessamentoPagamento(request.NumeroCartao);

        if (pagamentoAprovado)
        {
            pagamento.Confirmar();
        }
        else
        {
            pagamento.Rejeitar("Saldo insuficiente ou cartão inválido");
        }

        // Persistir
        _pagamentoRepository.Adicionar(pagamento);

        // Commit (Unit of Work)
        return await _pagamentoRepository.UnitOfWork.Commit();
    }

    /// <summary>
    /// Handler para confirmar um pagamento
    /// </summary>
    public async Task<bool> Handle(ConfirmarPagamentoCommand request, CancellationToken cancellationToken)
    {
        if (!request.EhValido())
        {
            return false;
        }

        var pagamento = await _pagamentoRepository.ObterPorId(request.PagamentoId);

        if (pagamento == null)
        {
            return false;
        }

        pagamento.Confirmar();

        _pagamentoRepository.Atualizar(pagamento);

        return await _pagamentoRepository.UnitOfWork.Commit();
    }

    /// <summary>
    /// Simula processamento de pagamento
    /// Em produção, seria integração com PagSeguro, Stripe, etc.
    /// </summary>
    private bool SimularProcessamentoPagamento(string numeroCartao)
    {
        // Simulação simples: cartões terminados em número par = aprovado
        var ultimoDigito = int.Parse(numeroCartao.Substring(numeroCartao.Length - 1));
        return ultimoDigito % 2 == 0;
    }
}

