using MBA.Educacao.Online.Core.Data;
using MBA.Educacao.Online.Core.Messages;
using MBA.Educacao.Online.GestaoAlunos.Domain;
using Microsoft.EntityFrameworkCore;

namespace MBA.Educacao.Online.GestaoAlunos.Data;

public class AlunoContext : DbContext, IUnitOfWork
{
    public AlunoContext(DbContextOptions<AlunoContext> options)
        : base(options) { }

    public DbSet<Aluno> Alunos { get; set; }
    public DbSet<Matricula> Matriculas { get; set; }
    public DbSet<Certificado> Certificados { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Ignore<Event>();
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(AlunoContext).Assembly);
        base.OnModelCreating(modelBuilder);
    }

    public async Task<bool> Commit()
    {
        foreach (var entry in ChangeTracker.Entries()
            .Where(entry => entry.Entity.GetType().GetProperty("DataCadastro") != null))
        {
            if (entry.State == EntityState.Added)
            {
                entry.Property("DataCadastro").CurrentValue = DateTime.Now;
            }

            if (entry.State == EntityState.Modified)
            {
                entry.Property("DataCadastro").IsModified = false;
            }
        }

        return await base.SaveChangesAsync() > 0;
    }
}