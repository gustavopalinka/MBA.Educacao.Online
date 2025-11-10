using FluentAssertions;
using MBA.Educacao.Online.Core.Extensions;
using Xunit;

namespace MBA.Educacao.Online.UnitTests.Core;

public class StringExtensionsTests
{
    [Trait("Categoria", "Core - Utilitarios")]
    [Fact(DisplayName = "ToGuid deve retornar Guid válido")]
    public void ToGuid_Deve_Retornar_Guid_Valido()
    {
        var guid = Guid.NewGuid();

        var resultado = guid.ToString().ToGuid();

        resultado.Should().Be(guid);
    }

    [Trait("Categoria", "Core - Utilitarios")]
    [Fact(DisplayName = "ToGuid deve retornar Guid.Empty quando inválido")]
    public void ToGuid_Deve_Retornar_Guid_Empty_Quando_Invalido()
    {
        var resultado = "valor-invalido".ToGuid();

        resultado.Should().Be(Guid.Empty);
    }
}


