using MBA.Educacao.Online.Core.Data;

namespace MBA.Educacao.Online.GestaoConteudo.Domain;

/// <summary>
/// Interface do repositório de Curso
/// </summary>
public interface ICursoRepository : IRepository<Curso>
{
    Task<Curso?> ObterPorId(Guid id);
    Task<IEnumerable<Curso>> ObterTodos();
    Task<IEnumerable<Curso>> ObterCursosAtivos();
    Task<Curso?> ObterCursoComAulas(Guid id);
    void Adicionar(Curso curso);
    void Atualizar(Curso curso);
    void Remover(Curso curso);
}

