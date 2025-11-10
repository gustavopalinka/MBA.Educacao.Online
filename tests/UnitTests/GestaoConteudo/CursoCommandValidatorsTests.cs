using System;
using FluentAssertions;
using MBA.Educacao.Online.GestaoConteudo.Application.Commands;
using Xunit;

namespace MBA.Educacao.Online.UnitTests.GestaoConteudo;

public class CursoCommandValidatorsTests
{
    [Fact]
    public void CriarCursoCommand_Deve_Falhar_Quando_Dados_Invalidos()
    {
        var command = new CriarCursoCommand(
            string.Empty,
            string.Empty,
            -1m,
            0,
            string.Empty,
            string.Empty,
            string.Empty,
            string.Empty);

        var valido = command.EhValido();

        valido.Should().BeFalse();
        command.ValidationResult.Errors.Should().NotBeEmpty();
    }

    [Fact]
    public void AdicionarAulaCommand_Deve_Falhar_Quando_Dados_Invalidos()
    {
        var command = new AdicionarAulaCommand(
            Guid.Empty,
            string.Empty,
            string.Empty,
            string.Empty,
            0);

        var valido = command.EhValido();

        valido.Should().BeFalse();
        command.ValidationResult.Errors.Should().NotBeEmpty();
    }
}


