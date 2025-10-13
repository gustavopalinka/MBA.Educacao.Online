using MBA.Educacao.Online.GestaoConteudo.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MBA.Educacao.Online.GestaoConteudo.Data.Mappings;

public class AulaMapping : IEntityTypeConfiguration<Aula>
{
    public void Configure(EntityTypeBuilder<Aula> builder)
    {
        builder.ToTable("Aulas");
        builder.HasKey(a => a.Id);

        builder.Property(a => a.Codigo)
            .HasMaxLength(20)
            .IsUnicode(false)
            .IsRequired();

        builder.Property(a => a.Titulo)
               .HasMaxLength(200)
               .IsUnicode(false)
               .IsRequired();

        builder.Property(a => a.Descricao)
               .HasMaxLength(500)
               .IsUnicode(false)
               .IsRequired();

        builder.Property(a => a.Ordem)
               .IsRequired();

        builder.Property(a => a.CursoId)
               .IsRequired();

        builder.Property(a => a.DataCadastro)
               .IsRequired();

        builder.Property(a => a.Ativo)
               .IsRequired();

        builder.HasIndex(a => new { a.CursoId, a.Ordem });
    }
}