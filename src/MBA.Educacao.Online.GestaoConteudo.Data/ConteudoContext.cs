using MBA.Educacao.Online.Core.Data;
using MBA.Educacao.Online.Core.Messages;
using MBA.Educacao.Online.GestaoConteudo.Domain;
using Microsoft.EntityFrameworkCore;

namespace MBA.Educacao.Online.GestaoConteudo.Data;

public class ConteudoContext : DbContext, IUnitOfWork
{
    public ConteudoContext(DbContextOptions<ConteudoContext> options)
        : base(options) { }

    public DbSet<Curso> Cursos { get; set; }
    public DbSet<Aula> Aulas { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Ignore<Event>();
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(ConteudoContext).Assembly);
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

        var success = await base.SaveChangesAsync() > 0;
        return success;
    }
}