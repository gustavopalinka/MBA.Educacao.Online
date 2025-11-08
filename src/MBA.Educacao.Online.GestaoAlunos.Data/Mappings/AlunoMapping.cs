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
               .HasMaxLength(200)
               .IsRequired();

        builder.Property(a => a.Email)
               .HasMaxLength(200)
               .IsRequired();

        builder.Property(a => a.DataCadastro)
               .IsRequired();

        builder.Property(a => a.Ativo)
               .IsRequired();

        builder.OwnsOne(a => a.HistoricoAprendizado, historico =>
        {
            historico.Ignore(h => h.AulasConcluidas);

            historico.OwnsMany(h => h.AulasConcluidasEf, aulas =>
            {
                aulas.ToTable("AulasConcluidas");
                aulas.WithOwner().HasForeignKey("AlunoId");
                aulas.HasKey(a => a.Id);
                aulas.Property(a => a.CursoId).IsRequired();
                aulas.Property(a => a.AulaId).IsRequired();
                aulas.Property(a => a.DataAprendizado).IsRequired();
            });
        });

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