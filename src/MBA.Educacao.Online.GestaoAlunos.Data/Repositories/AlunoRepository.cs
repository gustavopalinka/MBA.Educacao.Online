using MBA.Educacao.Online.Core.Data;
using MBA.Educacao.Online.GestaoAlunos.Domain;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace MBA.Educacao.Online.GestaoAlunos.Data.Repositories;

public class AlunoRepository : IAlunoRepository
{
    private readonly AlunoContext _context;

    public AlunoRepository(AlunoContext context)
    {
        _context = context;
    }

    public IUnitOfWork UnitOfWork => _context;

    public async Task<Aluno?> ObterPorId(Guid id)
    {
        return await _context.Alunos
            .AsNoTracking()
            .FirstOrDefaultAsync(a => a.Id == id);
    }

    public async Task<Aluno?> ObterPorEmail(string email)
    {
        return await _context.Alunos
            .AsNoTracking()
            .FirstOrDefaultAsync(a => a.Email == email);
    }

    public async Task<Aluno?> ObterAlunoComMatriculas(Guid id)
    {
        return await _context.Alunos
            .Include(a => a.Matriculas)
            .Include(a => a.HistoricoAprendizado.AulasConcluidasEf)
            .FirstOrDefaultAsync(a => a.Id == id);
    }

    public async Task<Aluno?> ObterAlunoComCertificados(Guid id)
    {
        return await _context.Alunos
            .Include(a => a.Certificados)
            .Include(a => a.HistoricoAprendizado.AulasConcluidasEf)
            .FirstOrDefaultAsync(a => a.Id == id);
    }

    public async Task<IEnumerable<Aluno>> ObterTodos()
    {
        return await _context.Alunos
            .AsNoTracking()
            .ToListAsync();
    }

    public void Adicionar(Aluno aluno)
    {
        _context.Alunos.Add(aluno);
    }

    public void Atualizar(Aluno aluno)
    {
        var local = _context.Alunos.Local.FirstOrDefault(a => a.Id == aluno.Id);
        if (local is not null)
        {
            _context.Entry(local).State = EntityState.Detached;
        }

        _context.Alunos.Update(aluno);
    }

    public void Dispose()
    {
        _context?.Dispose();
    }
}

