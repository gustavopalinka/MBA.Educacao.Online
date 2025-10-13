using MBA.Educacao.Online.GestaoAlunos.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MBA.Educacao.Online.GestaoAlunos.Data.Mappings;

public class AlunoMapping : IEntityTypeConfiguration<Aluno>
{
    public void Configure(EntityTypeBuilder<Aluno> builder)
    {
        builder.ToTable("Alunos");
        builder.HasKey(a => a.Id);

        builder.Property(a => a.Nome)
               .HasMaxLength(Aluno.NomeMaxLength)
               .IsRequired();

        builder.Property(a => a.Email)
               .HasMaxLength(Aluno.EmailMaxLength)
               .IsRequired();

        builder.Property(a => a.DataCadastro)
               .IsRequired();

        builder.Property(a => a.Ativo)
               .IsRequired();

        // Value Object - HistoricoAprendizado (Owned Entity)
        builder.OwnsOne(a => a.HistoricoAprendizado, h =>
        {
            h.OwnsMany(ha => ha.AulasConcluidas, ac =>
            {
                ac.ToTable("HistoricoAulas");
                ac.WithOwner().HasForeignKey("AlunoId");
                ac.Property<Guid>("Id");
                ac.HasKey("Id");
                ac.Property(p => p.CursoId).IsRequired();
                ac.Property(p => p.AulaId).IsRequired();
                ac.Property(p => p.DataAprendizado).IsRequired();
            });
        });

        // Relacionamentos
        builder.HasMany(a => a.Matriculas)
               .WithOne(m => m.Aluno)
               .HasForeignKey(m => m.AlunoId)
               .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(a => a.Certificados)
               .WithOne(c => c.Aluno)
               .HasForeignKey(c => c.AlunoId)
               .OnDelete(DeleteBehavior.Cascade);

        builder.HasIndex(a => a.Email).IsUnique();
    }
}

