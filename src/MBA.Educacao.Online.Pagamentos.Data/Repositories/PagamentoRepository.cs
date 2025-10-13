using MBA.Educacao.Online.Core.Data;
using MBA.Educacao.Online.Pagamentos.Domain;
using Microsoft.EntityFrameworkCore;

namespace MBA.Educacao.Online.Pagamentos.Data.Repositories;

public class PagamentoRepository : IPagamentoRepository
{
    private readonly PagamentoContext _context;

    public PagamentoRepository(PagamentoContext context)
    {
        _context = context;
    }

    public IUnitOfWork UnitOfWork => _context;

    public async Task<Pagamento?> ObterPorId(Guid id)
    {
        return await _context.Pagamentos
            .AsNoTracking()
            .FirstOrDefaultAsync(p => p.Id == id);
    }

    public async Task<Pagamento?> ObterPorMatricula(Guid matriculaId)
    {
        return await _context.Pagamentos
            .AsNoTracking()
            .FirstOrDefaultAsync(p => p.MatriculaId == matriculaId);
    }

    public async Task<IEnumerable<Pagamento>> ObterPorAluno(Guid alunoId)
    {
        return await _context.Pagamentos
            .AsNoTracking()
            .Where(p => p.AlunoId == alunoId)
            .OrderByDescending(p => p.DataPagamento)
            .ToListAsync();
    }

    public void Adicionar(Pagamento pagamento)
    {
        _context.Pagamentos.Add(pagamento);
    }

    public void Atualizar(Pagamento pagamento)
    {
        _context.Pagamentos.Update(pagamento);
    }

    public void Dispose()
    {
        _context?.Dispose();
    }
}

