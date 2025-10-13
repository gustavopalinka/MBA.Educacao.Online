namespace MBA.Educacao.Online.Pagamentos.Domain;

public class StatusPagamento
{
    protected StatusPagamento() { }

    public StatusPagamento(StatusPagamentoEnum status, string? motivoRejeicao = null)
    {
        Status = status;
        MotivoRejeicao = motivoRejeicao;
    }

    public StatusPagamentoEnum Status { get; private set; }
    public string? MotivoRejeicao { get; private set; }

    public bool EhFinal => Status == StatusPagamentoEnum.Confirmado || 
                           Status == StatusPagamentoEnum.Rejeitado || 
                           Status == StatusPagamentoEnum.Cancelado;

    public override bool Equals(object? obj)
    {
        if (obj is not StatusPagamento other) return false;
        return Status == other.Status;
    }

    public override int GetHashCode()
    {
        return Status.GetHashCode();
    }

    public override string ToString() => Status.ToString();
}

public enum StatusPagamentoEnum
{
    Pendente = 0,
    Processando = 1,
    Confirmado = 2,
    Rejeitado = 3,
    Cancelado = 4
}