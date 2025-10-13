using MBA.Educacao.Online.GestaoConteudo.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MBA.Educacao.Online.GestaoConteudo.Data.Mappings;

/// <summary>
/// Mapeamento EF Core para a entidade Curso
/// </summary>
public class CursoMapping : IEntityTypeConfiguration<Curso>
{
    public void Configure(EntityTypeBuilder<Curso> builder)
    {
        builder.ToTable("Cursos");
        builder.HasKey(c => c.Id);

        // Propriedades básicas
        builder.Property(c => c.Nome)
               .HasMaxLength(Curso.NomeMaxLength)
               .IsUnicode(false)
               .IsRequired();

        builder.Property(c => c.Descricao)
               .HasMaxLength(Curso.DescricaoMaxLength)
               .IsUnicode(false)
               .IsRequired();

        builder.Property(c => c.Valor)
               .HasColumnType("decimal(18,2)")
               .IsRequired();

        builder.Property(c => c.CargaHoraria)
               .IsRequired();

        builder.Property(c => c.PublicoAlvo)
               .HasMaxLength(Curso.PublicoAlvoMaxLength)
               .IsUnicode(false)
               .IsRequired();

        builder.Property(c => c.Objetivo)
               .HasMaxLength(Curso.ObjetivoMaxLength)
               .IsUnicode(false)
               .IsRequired();

        builder.Property(c => c.Requisitos)
               .HasMaxLength(Curso.RequisitosMaxLength)
               .IsUnicode(false)
               .IsRequired();

        builder.Property(c => c.DataCadastro)
               .IsRequired();

        builder.Property(c => c.Ativo)
               .IsRequired();

        // Value Object - ConteudoProgramatico (Owned Entity)
        builder.OwnsOne(c => c.ConteudoProgramatico, cp =>
        {
            cp.Property(p => p.ConteudoDescricao)
              .HasColumnName("ConteudoDescricao")
              .HasMaxLength(ConteudoProgramatico.DescricaoMaxLength)
              .IsUnicode(false)
              .IsRequired();

            cp.Property(p => p.Revisao)
              .HasColumnName("Revisao")
              .IsRequired();

            cp.Property(p => p.DataRevisao)
              .HasColumnName("DataRevisao")
              .IsRequired();
        });

        // Relacionamento com Aulas
        builder.HasMany(c => c.Aulas)
               .WithOne(a => a.Curso)
               .HasForeignKey(a => a.CursoId)
               .OnDelete(DeleteBehavior.Cascade);

        // Índices para performance
        builder.HasIndex(c => c.Nome);
        builder.HasIndex(c => c.Ativo);
    }
}

