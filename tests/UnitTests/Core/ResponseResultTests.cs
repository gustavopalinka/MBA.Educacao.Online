using FluentAssertions;
using MBA.Educacao.Online.Core.Mediator;
using Xunit;

namespace MBA.Educacao.Online.UnitTests.Core;

public class ResponseResultTests
{
    [Fact]
    public void IsValid_Deve_Ser_Verdadeiro_Quando_Sem_Erros()
    {
        var result = new ResponseResult();

        result.IsValid.Should().BeTrue();
    }

    [Fact]
    public void AddError_Deve_Registrar_Erro()
    {
        var result = new ResponseResult();

        result.AddError("Falha");

        result.IsValid.Should().BeFalse();
        result.Errors.Should().ContainSingle(e => e == "Falha");
    }
}


