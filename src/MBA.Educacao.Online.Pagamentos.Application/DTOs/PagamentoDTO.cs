namespace MBA.Educacao.Online.Pagamentos.Application.DTOs;

/// <summary>
/// DTO para retorno de dados de Pagamento
/// </summary>
public class PagamentoDTO
{
    public Guid Id { get; set; }
    public Guid MatriculaId { get; set; }
    public Guid AlunoId { get; set; }
    public decimal Valor { get; set; }
    public DateTime DataPagamento { get; set; }
    public DateTime? DataConfirmacao { get; set; }
    public string NumeroCartao { get; set; } = string.Empty; // Mascarado
    public string NomeTitular { get; set; } = string.Empty;
    public string Status { get; set; } = string.Empty;
    public string? MotivoRejeicao { get; set; }
}

