using MBA.Educacao.Online.Core.Messages;

namespace MBA.Educacao.Online.Pagamentos.Domain;

public class PagamentoConfirmadoEvent : Event
{
    public PagamentoConfirmadoEvent(Guid pagamentoId, Guid matriculaId, Guid alunoId)
    {
        AggregateId = pagamentoId;
        MatriculaId = matriculaId;
        AlunoId = alunoId;
    }

    public Guid MatriculaId { get; private set; }
    public Guid AlunoId { get; private set; }
}

public class PagamentoRejeitadoEvent : Event
{
    public PagamentoRejeitadoEvent(Guid pagamentoId, Guid matriculaId, Guid alunoId, string motivo)
    {
        AggregateId = pagamentoId;
        MatriculaId = matriculaId;
        AlunoId = alunoId;
        Motivo = motivo;
    }

    public Guid MatriculaId { get; private set; }
    public Guid AlunoId { get; private set; }
    public string Motivo { get; private set; }
}