using MBA.Educacao.Online.GestaoAlunos.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MBA.Educacao.Online.GestaoAlunos.Data.Mappings;

public class CertificadoMapping : IEntityTypeConfiguration<Certificado>
{
    public void Configure(EntityTypeBuilder<Certificado> builder)
    {
        builder.ToTable("Certificados");
        builder.HasKey(c => c.Id);

        builder.Property(c => c.AlunoId).IsRequired();
        builder.Property(c => c.CursoId).IsRequired();
        builder.Property(c => c.DataEmissao).IsRequired();
        
        builder.Property(c => c.Codigo)
               .HasMaxLength(Certificado.CodigoMaxLength)
               .IsRequired();

        builder.Property(c => c.Valido).IsRequired();

        builder.HasIndex(c => c.Codigo).IsUnique();
        builder.HasIndex(c => new { c.AlunoId, c.CursoId });
    }
}

