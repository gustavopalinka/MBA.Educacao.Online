using MBA.Educacao.Online.Core.Data;
using MBA.Educacao.Online.GestaoConteudo.Domain;
using Microsoft.EntityFrameworkCore;

namespace MBA.Educacao.Online.GestaoConteudo.Data.Repositories;

/// <summary>
/// Implementação do repositório de Curso
/// </summary>
public class CursoRepository : ICursoRepository
{
    private readonly ConteudoContext _context;

    public CursoRepository(ConteudoContext context)
    {
        _context = context;
    }

    public IUnitOfWork UnitOfWork => _context;

    public async Task<Curso?> ObterPorId(Guid id)
    {
        return await _context.Cursos
            .AsNoTracking()
            .FirstOrDefaultAsync(c => c.Id == id);
    }

    public async Task<IEnumerable<Curso>> ObterTodos()
    {
        return await _context.Cursos
            .AsNoTracking()
            .OrderBy(c => c.Nome)
            .ToListAsync();
    }

    public async Task<IEnumerable<Curso>> ObterCursosAtivos()
    {
        return await _context.Cursos
            .AsNoTracking()
            .Where(c => c.Ativo)
            .OrderBy(c => c.Nome)
            .ToListAsync();
    }

    public async Task<Curso?> ObterCursoComAulas(Guid id)
    {
        return await _context.Cursos
            .Include(c => c.Aulas.OrderBy(a => a.Ordem))
            .AsNoTracking()
            .FirstOrDefaultAsync(c => c.Id == id);
    }

    public void Adicionar(Curso curso)
    {
        _context.Cursos.Add(curso);
    }

    public void Atualizar(Curso curso)
    {
        _context.Cursos.Update(curso);
    }

    public void Remover(Curso curso)
    {
        _context.Cursos.Remove(curso);
    }

    public void Dispose()
    {
        _context?.Dispose();
    }
}

