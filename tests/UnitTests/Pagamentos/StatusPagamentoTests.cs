using FluentAssertions;
using MBA.Educacao.Online.Pagamentos.Domain;
using Xunit;

namespace MBA.Educacao.Online.UnitTests.Pagamentos;

public class StatusPagamentoTests
{
    [Trait("Categoria", "Pagamentos - Dominio")]
    [Fact(DisplayName = "EhFinal deve ser verdadeiro para status finalizados")]
    public void EhFinal_Deve_Retornar_Verdadeiro_Para_Status_Finalizados()
    {
        new StatusPagamento(StatusPagamentoEnum.Confirmado).EhFinal.Should().BeTrue();
        new StatusPagamento(StatusPagamentoEnum.Rejeitado, "Motivo").EhFinal.Should().BeTrue();
        new StatusPagamento(StatusPagamentoEnum.Cancelado).EhFinal.Should().BeTrue();
    }

    [Trait("Categoria", "Pagamentos - Dominio")]
    [Fact(DisplayName = "EhFinal deve ser falso para status não finalizados")]
    public void EhFinal_Deve_Retornar_Falso_Para_Status_Nao_Finalizados()
    {
        new StatusPagamento(StatusPagamentoEnum.Pendente).EhFinal.Should().BeFalse();
        new StatusPagamento(StatusPagamentoEnum.Processando).EhFinal.Should().BeFalse();
    }

    [Trait("Categoria", "Pagamentos - Dominio")]
    [Fact(DisplayName = "Equals deve comparar apenas por status")]
    public void Equals_Deve_Comparar_Apenas_Pelo_Status()
    {
        var status1 = new StatusPagamento(StatusPagamentoEnum.Rejeitado, "Motivo A");
        var status2 = new StatusPagamento(StatusPagamentoEnum.Rejeitado, "Motivo B");

        status1.Should().Be(status2);
        status1.GetHashCode().Should().Be(status2.GetHashCode());
    }
}


