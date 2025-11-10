using System;
using FluentAssertions;
using MBA.Educacao.Online.Core.DomainObjects;
using MBA.Educacao.Online.Pagamentos.Domain;
using Xunit;

namespace MBA.Educacao.Online.UnitTests.Pagamentos;

public class PagamentoDomainTests
{
    [Trait("Categoria", "Pagamentos - Dominio")]
    [Fact(DisplayName = "Deve confirmar pagamento e publicar evento")]
    public void Deve_Confirmar_Pagamento()
    {
        var pagamento = CriarPagamento("5555444433332222");

        pagamento.Confirmar();

        pagamento.EstaConfirmado().Should().BeTrue();
        pagamento.Notificacoes.Should().ContainSingle()
            .Which.Should().BeOfType<PagamentoConfirmadoEvent>();
    }

    [Trait("Categoria", "Pagamentos - Dominio")]
    [Fact(DisplayName = "Deve cancelar pagamento")]
    public void Deve_Cancelar_Pagamento()
    {
        var pagamento = CriarPagamento("5555444433332222");

        pagamento.Cancelar();

        pagamento.StatusPagamento.Status.Should().Be(StatusPagamentoEnum.Cancelado);
        pagamento.Notificacoes.Should().BeNullOrEmpty();
    }

    [Trait("Categoria", "Pagamentos - Dominio")]
    [Fact(DisplayName = "Deve rejeitar pagamento e publicar evento")]
    public void Deve_Rejeitar_Pagamento()
    {
        var pagamento = CriarPagamento("5555444433332222");

        pagamento.Rejeitar("Cartão inválido");

        pagamento.Notificacoes.Should().ContainSingle()
            .Which.Should().BeOfType<PagamentoRejeitadoEvent>();
        pagamento.StatusPagamento.Status.Should().Be(StatusPagamentoEnum.Rejeitado);
    }

    [Trait("Categoria", "Pagamentos - Dominio")]
    [Fact(DisplayName = "Deve lançar exceção quando valor negativo")]
    public void Deve_Lancar_Excecao_Quando_Valor_Negativo()
    {
        Action act = () => new Pagamento(Guid.NewGuid(), Guid.NewGuid(), -10m,
            new DadosCartao("5555444433332222", "Aluno Teste", "12/30", "123"));

        act.Should().Throw<DomainException>()
            .WithMessage("O valor do pagamento não pode ser negativo");
    }

    [Trait("Categoria", "Pagamentos - Dominio")]
    [Fact(DisplayName = "Deve limpar eventos após processamento")]
    public void Deve_Limpar_Eventos_Apos_Processamento()
    {
        var pagamento = CriarPagamento("5555444433332222");

        pagamento.Confirmar();
        pagamento.Notificacoes.Should().NotBeNull();

        pagamento.LimparEventos();

        pagamento.Notificacoes.Should().BeNullOrEmpty();
    }

    private static Pagamento CriarPagamento(string numeroCartao)
    {
        return new Pagamento(Guid.NewGuid(), Guid.NewGuid(), 150m,
            new DadosCartao(numeroCartao, "Aluno Teste", "12/30", "123"));
    }
}


