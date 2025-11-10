using System;
using FluentAssertions;
using MBA.Educacao.Online.Core.DomainObjects;
using MBA.Educacao.Online.Pagamentos.Domain;
using Xunit;

namespace MBA.Educacao.Online.UnitTests.Pagamentos;

public class PagamentoDomainTests
{
    [Fact]
    public void Deve_Confirmar_Pagamento()
    {
        var pagamento = CriarPagamento("5555444433332222");

        pagamento.Confirmar();

        pagamento.EstaConfirmado().Should().BeTrue();
        pagamento.Notificacoes.Should().ContainSingle()
            .Which.Should().BeOfType<PagamentoConfirmadoEvent>();
    }

    [Fact]
    public void Deve_Rejeitar_Pagamento()
    {
        var pagamento = CriarPagamento("5555444433332222");

        pagamento.Rejeitar("Cartão inválido");

        pagamento.Notificacoes.Should().ContainSingle()
            .Which.Should().BeOfType<PagamentoRejeitadoEvent>();
        pagamento.StatusPagamento.Status.Should().Be(StatusPagamentoEnum.Rejeitado);
    }

    [Fact]
    public void Deve_Lancar_Excecao_Quando_Valor_Negativo()
    {
        Action act = () => new Pagamento(Guid.NewGuid(), Guid.NewGuid(), -10m,
            new DadosCartao("5555444433332222", "Aluno Teste", "12/30", "123"));

        act.Should().Throw<DomainException>()
            .WithMessage("O valor do pagamento não pode ser negativo");
    }

    private static Pagamento CriarPagamento(string numeroCartao)
    {
        return new Pagamento(Guid.NewGuid(), Guid.NewGuid(), 150m,
            new DadosCartao(numeroCartao, "Aluno Teste", "12/30", "123"));
    }
}


