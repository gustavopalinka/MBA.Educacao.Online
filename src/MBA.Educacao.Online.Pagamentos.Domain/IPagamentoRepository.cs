using MBA.Educacao.Online.Core.Data;

namespace MBA.Educacao.Online.Pagamentos.Domain;

/// <summary>
/// Interface do repositório de Pagamento
/// </summary>
public interface IPagamentoRepository : IRepository<Pagamento>
{
    Task<Pagamento?> ObterPorId(Guid id);
    Task<Pagamento?> ObterPorMatricula(Guid matriculaId);
    Task<IEnumerable<Pagamento>> ObterPorAluno(Guid alunoId);
    void Adicionar(Pagamento pagamento);
    void Atualizar(Pagamento pagamento);
}
