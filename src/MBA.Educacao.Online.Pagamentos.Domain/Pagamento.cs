using MBA.Educacao.Online.Core.DomainObjects;

namespace MBA.Educacao.Online.Pagamentos.Domain;

/// <summary>
/// Aggregate Root: Pagamento
/// Representa um pagamento de matrícula
/// </summary>
public class Pagamento : Entity, IAggregateRoot
{
    protected Pagamento() { }

    public Pagamento(Guid matriculaId, Guid alunoId, decimal valor, DadosCartao dadosCartao)
    {
        Validacoes.ValidarSeMenorQue(valor, 0, "O valor do pagamento não pode ser negativo");
        Validacoes.ValidarSeNulo(dadosCartao, "Os dados do cartão são obrigatórios");
        
        MatriculaId = matriculaId;
        AlunoId = alunoId;
        Valor = valor;
        DadosCartao = dadosCartao;
        DataPagamento = DateTime.Now;
        StatusPagamento = new StatusPagamento(StatusPagamentoEnum.Pendente);
    }

    public Guid MatriculaId { get; private set; }
    public Guid AlunoId { get; private set; }
    public decimal Valor { get; private set; }
    public DateTime DataPagamento { get; private set; }
    public DateTime? DataConfirmacao { get; private set; }
    
    // Value Objects
    public DadosCartao DadosCartao { get; private set; }
    public StatusPagamento StatusPagamento { get; private set; }

    // Comportamentos
    public void Confirmar()
    {
        StatusPagamento = new StatusPagamento(StatusPagamentoEnum.Confirmado);
        DataConfirmacao = DateTime.Now;
        
        // Dispara evento de domínio
        AdicionarEvento(new PagamentoConfirmadoEvent(Id, MatriculaId, AlunoId));
    }

    public void Rejeitar(string motivo)
    {
        StatusPagamento = new StatusPagamento(StatusPagamentoEnum.Rejeitado, motivo);
        
        // Dispara evento de domínio
        AdicionarEvento(new PagamentoRejeitadoEvent(Id, MatriculaId, AlunoId, motivo));
    }

    public void Cancelar()
    {
        StatusPagamento = new StatusPagamento(StatusPagamentoEnum.Cancelado);
    }

    public bool EstaConfirmado() => StatusPagamento.Status == StatusPagamentoEnum.Confirmado;
}
