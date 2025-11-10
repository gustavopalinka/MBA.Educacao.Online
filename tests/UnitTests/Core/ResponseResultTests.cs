using FluentAssertions;
using MBA.Educacao.Online.Core.Mediator;
using Xunit;

namespace MBA.Educacao.Online.UnitTests.Core;

public class ResponseResultTests
{
    [Trait("Categoria", "Core - Utilitarios")]
    [Fact(DisplayName = "ResponseResult é válido quando não possui erros")]
    public void IsValid_Deve_Ser_Verdadeiro_Quando_Sem_Erros()
    {
        var result = new ResponseResult();

        result.IsValid.Should().BeTrue();
    }

    [Trait("Categoria", "Core - Utilitarios")]
    [Fact(DisplayName = "AddError deve registrar erro")]
    public void AddError_Deve_Registrar_Erro()
    {
        var result = new ResponseResult();

        result.AddError("Falha");

        result.IsValid.Should().BeFalse();
        result.Errors.Should().ContainSingle(e => e == "Falha");
    }
}


