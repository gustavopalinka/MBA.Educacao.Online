using MBA.Educacao.Online.Core.Data;
using MBA.Educacao.Online.Core.Messages;
using MBA.Educacao.Online.Pagamentos.Domain;
using Microsoft.EntityFrameworkCore;

namespace MBA.Educacao.Online.Pagamentos.Data;

public class PagamentoContext : DbContext, IUnitOfWork
{
    public PagamentoContext(DbContextOptions<PagamentoContext> options)
        : base(options) { }

    public DbSet<Pagamento> Pagamentos { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Ignore<Event>();
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(PagamentoContext).Assembly);
        base.OnModelCreating(modelBuilder);
    }

    public async Task<bool> Commit()
    {
        foreach (var entry in ChangeTracker.Entries()
            .Where(entry => entry.Entity.GetType().GetProperty("DataPagamento") != null))
        {
            if (entry.State == EntityState.Added)
            {
                entry.Property("DataPagamento").CurrentValue = DateTime.Now;
            }

            if (entry.State == EntityState.Modified)
            {
                entry.Property("DataPagamento").IsModified = false;
            }
        }

        return await base.SaveChangesAsync() > 0;
    }
}