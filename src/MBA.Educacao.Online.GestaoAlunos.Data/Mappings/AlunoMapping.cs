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

        builder.OwnsOne(a => a.HistoricoAprendizado, h =>
        {
            h.Ignore(ha => ha.AulasConcluidas);
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