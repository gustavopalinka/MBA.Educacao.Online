namespace MBA.Educacao.Online.Pagamentos.Application.DTOs;

public class PagamentoDTO
{
    public Guid Id { get; set; }
    public Guid MatriculaId { get; set; }
    public Guid AlunoId { get; set; }
    public decimal Valor { get; set; }
    public DateTime DataPagamento { get; set; }
    public DateTime? DataConfirmacao { get; set; }
    public string NumeroCartao { get; set; } = string.Empty;
    public string NomeTitular { get; set; } = string.Empty;
    public string Status { get; set; } = string.Empty;
    public string? MotivoRejeicao { get; set; }
}

