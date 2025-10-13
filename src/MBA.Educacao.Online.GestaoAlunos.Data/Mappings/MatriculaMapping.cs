using MBA.Educacao.Online.GestaoAlunos.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MBA.Educacao.Online.GestaoAlunos.Data.Mappings;

public class MatriculaMapping : IEntityTypeConfiguration<Matricula>
{
    public void Configure(EntityTypeBuilder<Matricula> builder)
    {
        builder.ToTable("Matriculas");
        builder.HasKey(m => m.Id);

        builder.Property(m => m.AlunoId).IsRequired();
        builder.Property(m => m.CursoId).IsRequired();
        builder.Property(m => m.DataMatricula).IsRequired();
        builder.Property(m => m.DataValidade).IsRequired();
        builder.Property(m => m.DataConclusao);
        
        builder.Property(m => m.Status)
               .HasConversion<int>()
               .IsRequired();

        builder.Property(m => m.Ativo).IsRequired();

        builder.HasIndex(m => new { m.AlunoId, m.CursoId });
        builder.HasIndex(m => m.Status);
    }
}

