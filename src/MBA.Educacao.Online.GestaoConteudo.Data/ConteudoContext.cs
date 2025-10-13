using MBA.Educacao.Online.Core.Data;
using MBA.Educacao.Online.Core.Messages;
using MBA.Educacao.Online.GestaoConteudo.Domain;
using Microsoft.EntityFrameworkCore;

namespace MBA.Educacao.Online.GestaoConteudo.Data;

/// <summary>
/// DbContext para o Bounded Context de Gestão de Conteúdo
/// </summary>
public class ConteudoContext : DbContext, IUnitOfWork
{
    public ConteudoContext(DbContextOptions<ConteudoContext> options)
        : base(options) { }

    public DbSet<Curso> Cursos { get; set; }
    public DbSet<Aula> Aulas { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Ignora eventos de domínio (não persistimos eventos no banco)
        modelBuilder.Ignore<Event>();

        // Aplica todas as configurações de mapeamento do assembly
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(ConteudoContext).Assembly);
        
        base.OnModelCreating(modelBuilder);
    }

    /// <summary>
    /// Implementação do Unit of Work para persistir mudanças
    /// </summary>
    public async Task<bool> Commit()
    {
        // Define DataCadastro automaticamente ao adicionar entidades
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

