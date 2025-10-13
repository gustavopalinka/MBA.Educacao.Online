using MBA.Educacao.Online.GestaoConteudo.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MBA.Educacao.Online.GestaoConteudo.Data.Mappings;

public class CursoMapping : IEntityTypeConfiguration<Curso>
{
    public void Configure(EntityTypeBuilder<Curso> builder)
    {
        builder.ToTable("Cursos");
        builder.HasKey(c => c.Id);

        builder.Property(c => c.Nome)
               .HasMaxLength(200)
               .IsUnicode(false)
               .IsRequired();

        builder.Property(c => c.Descricao)
               .HasMaxLength(1000)
               .IsUnicode(false)
               .IsRequired();

        builder.Property(c => c.Valor)
               .HasColumnType("decimal(18,2)")
               .IsRequired();

        builder.Property(c => c.CargaHoraria)
               .IsRequired();

        builder.Property(c => c.PublicoAlvo)
               .HasMaxLength(300)
               .IsUnicode(false)
               .IsRequired();

        builder.Property(c => c.Objetivo)
               .HasMaxLength(500)
               .IsUnicode(false)
               .IsRequired();

        builder.Property(c => c.Requisitos)
               .HasMaxLength(500)
               .IsUnicode(false)
               .IsRequired();

        builder.Property(c => c.DataCadastro)
               .IsRequired();

        builder.Property(c => c.Ativo)
               .IsRequired();

        builder.OwnsOne(c => c.ConteudoProgramatico, cp =>
        {
            cp.Property(p => p.ConteudoDescricao)
              .HasColumnName("ConteudoDescricao")
              .HasMaxLength(1000)
              .IsUnicode(false)
              .IsRequired();

            cp.Property(p => p.Revisao)
              .HasColumnName("Revisao")
              .IsRequired();

            cp.Property(p => p.DataRevisao)
              .HasColumnName("DataRevisao")
              .IsRequired();
        });

        builder.HasMany(c => c.Aulas)
               .WithOne(a => a.Curso)
               .HasForeignKey(a => a.CursoId)
               .OnDelete(DeleteBehavior.Cascade);

        builder.HasIndex(c => c.Nome);
        builder.HasIndex(c => c.Ativo);
    }
}