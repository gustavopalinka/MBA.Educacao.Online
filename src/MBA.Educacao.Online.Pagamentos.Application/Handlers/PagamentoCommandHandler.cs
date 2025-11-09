using System.Threading;
using System.Threading.Tasks;
using MBA.Educacao.Online.Core.Mediator;
using MBA.Educacao.Online.Pagamentos.Application.Commands;
using MBA.Educacao.Online.Pagamentos.Domain;
using MediatR;

namespace MBA.Educacao.Online.Pagamentos.Application.Handlers;

public class PagamentoCommandHandler :
    CommandHandler,
    IRequestHandler<RealizarPagamentoCommand, bool>,
    IRequestHandler<ConfirmarPagamentoCommand, bool>
{
    private readonly IPagamentoRepository _pagamentoRepository;

    public PagamentoCommandHandler(IPagamentoRepository pagamentoRepository,
                                   IMediatorHandler mediatorHandler)
        : base(mediatorHandler)
    {
        _pagamentoRepository = pagamentoRepository;
    }

    public async Task<bool> Handle(RealizarPagamentoCommand request, CancellationToken cancellationToken)
    {
        if (!request.EhValido())
        {
            await NotificarErros(request.ValidationResult);
            return false;
        }

        var dadosCartao = new DadosCartao(
            request.NumeroCartao,
            request.NomeTitular,
            request.Validade,
            request.CVV
        );

        var pagamento = new Pagamento(
            request.MatriculaId,
            request.AlunoId,
            request.Valor,
            dadosCartao
        );

        var pagamentoAprovado = SimularProcessamentoPagamento(request.NumeroCartao);

        if (pagamentoAprovado)
        {
            pagamento.Confirmar();
        }
        else
        {
            pagamento.Rejeitar("Saldo insuficiente ou cartão inválido");
        }

        _pagamentoRepository.Adicionar(pagamento);

        var sucesso = await _pagamentoRepository.UnitOfWork.Commit();

        if (!sucesso)
        {
            await NotificarErro(request.MessageType, "Falha ao registrar o pagamento.");
            return false;
        }

        await PublicarEventos(pagamento);

        return true;
    }

    public async Task<bool> Handle(ConfirmarPagamentoCommand request, CancellationToken cancellationToken)
    {
        if (!request.EhValido())
        {
            await NotificarErros(request.ValidationResult);
            return false;
        }

        var pagamento = await _pagamentoRepository.ObterPorId(request.PagamentoId);

        if (pagamento == null)
        {
            await NotificarErro(request.MessageType, "Pagamento não encontrado.");
            return false;
        }

        pagamento.Confirmar();

        _pagamentoRepository.Atualizar(pagamento);

        var sucesso = await _pagamentoRepository.UnitOfWork.Commit();

        if (!sucesso)
        {
            await NotificarErro(request.MessageType, "Falha ao confirmar o pagamento.");
            return false;
        }

        await PublicarEventos(pagamento);

        return true;
    }

    private bool SimularProcessamentoPagamento(string numeroCartao)
    {
        var ultimoDigito = int.Parse(numeroCartao.Substring(numeroCartao.Length - 1));
        return ultimoDigito % 2 == 0;
    }

    private async Task PublicarEventos(Pagamento pagamento)
    {
        if (pagamento.Notificacoes is null)
        {
            return;
        }

        foreach (var domainEvent in pagamento.Notificacoes)
        {
            await MediatorHandler.PublicarEvento(domainEvent);
        }

        pagamento.LimparEventos();
    }
}

