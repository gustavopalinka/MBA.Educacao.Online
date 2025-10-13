using MBA.Educacao.Online.Core.Data;

namespace MBA.Educacao.Online.GestaoConteudo.Domain;

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