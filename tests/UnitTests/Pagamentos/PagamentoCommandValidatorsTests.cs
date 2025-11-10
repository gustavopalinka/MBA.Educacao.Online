using System;
using FluentAssertions;
using MBA.Educacao.Online.Pagamentos.Application.Commands;
using Xunit;

namespace MBA.Educacao.Online.UnitTests.Pagamentos;

public class PagamentoCommandValidatorsTests
{
    [Trait("Categoria", "Pagamentos - Validators")]
    [Fact(DisplayName = "RealizarPagamentoCommand inválido quando dados inconsistentes")]
    public void RealizarPagamentoCommand_Deve_Falhar_Quando_Dados_Invalidos()
    {
        var command = new RealizarPagamentoCommand(
            Guid.Empty,
            Guid.Empty,
            -10m,
            string.Empty,
            string.Empty,
            string.Empty,
            string.Empty);

        var valido = command.EhValido();

        valido.Should().BeFalse();
        command.ValidationResult.Errors.Should().NotBeEmpty();
    }

    [Trait("Categoria", "Pagamentos - Validators")]
    [Fact(DisplayName = "ConfirmarPagamentoCommand inválido quando Id vazio")]
    public void ConfirmarPagamentoCommand_Deve_Falhar_Quando_Dados_Invalidos()
    {
        var command = new ConfirmarPagamentoCommand(Guid.Empty);

        var valido = command.EhValido();

        valido.Should().BeFalse();
        command.ValidationResult.Errors.Should().NotBeEmpty();
    }
}


