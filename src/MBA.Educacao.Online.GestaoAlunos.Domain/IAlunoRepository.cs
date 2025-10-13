using MBA.Educacao.Online.Core.Data;

namespace MBA.Educacao.Online.GestaoAlunos.Domain;

/// <summary>
/// Interface do repositório de Aluno
/// </summary>
public interface IAlunoRepository : IRepository<Aluno>
{
    Task<Aluno?> ObterPorId(Guid id);
    Task<Aluno?> ObterPorEmail(string email);
    Task<Aluno?> ObterAlunoComMatriculas(Guid id);
    Task<Aluno?> ObterAlunoComCertificados(Guid id);
    Task<IEnumerable<Aluno>> ObterTodos();
    void Adicionar(Aluno aluno);
    void Atualizar(Aluno aluno);
}

