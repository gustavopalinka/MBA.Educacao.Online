using MBA.Educacao.Online.Pagamentos.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MBA.Educacao.Online.Pagamentos.Data.Mappings;

public class PagamentoMapping : IEntityTypeConfiguration<Pagamento>
{
    public void Configure(EntityTypeBuilder<Pagamento> builder)
    {
        builder.ToTable("Pagamentos");
        builder.HasKey(p => p.Id);

        builder.Property(p => p.MatriculaId).IsRequired();
        builder.Property(p => p.AlunoId).IsRequired();
        
        builder.Property(p => p.Valor)
               .HasColumnType("decimal(18,2)")
               .IsRequired();

        builder.Property(p => p.DataPagamento).IsRequired();
        builder.Property(p => p.DataConfirmacao);

        // Value Object - DadosCartao (Owned Entity)
        builder.OwnsOne(p => p.DadosCartao, dc =>
        {
            dc.Property(d => d.NumeroCartao)
              .HasColumnName("NumeroCartao")
              .HasMaxLength(50)
              .IsRequired();

            dc.Property(d => d.NomeTitular)
              .HasColumnName("NomeTitular")
              .HasMaxLength(200)
              .IsRequired();

            dc.Property(d => d.Validade)
              .HasColumnName("Validade")
              .HasMaxLength(7)
              .IsRequired();

            dc.Property(d => d.CVV)
              .HasColumnName("CVV")
              .HasMaxLength(4)
              .IsRequired();
        });

        // Value Object - StatusPagamento (Owned Entity)
        builder.OwnsOne(p => p.StatusPagamento, sp =>
        {
            sp.Property(s => s.Status)
              .HasColumnName("Status")
              .HasConversion<int>()
              .IsRequired();

            sp.Property(s => s.MotivoRejeicao)
              .HasColumnName("MotivoRejeicao")
              .HasMaxLength(500);
        });

        // Índices
        builder.HasIndex(p => p.MatriculaId);
        builder.HasIndex(p => p.AlunoId);
        builder.HasIndex(p => p.DataPagamento);
    }
}